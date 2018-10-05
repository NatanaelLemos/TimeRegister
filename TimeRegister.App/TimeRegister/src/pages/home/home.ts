import { Component } from '@angular/core';
import { NavController, LoadingController } from 'ionic-angular';
import { SQLite, SQLiteObject } from '@ionic-native/sqlite';
import { Http } from '@angular/http';

@Component({
    selector: 'page-home',
    templateUrl: 'home.html'
})
export class HomePage {

    _loading: any;
    serverUrl: string = "http://10.0.0.244:5000/v1/time";

    constructor(public navCtrl: NavController, private sqlite: SQLite, private loadingCtrl: LoadingController, private http: Http) { }

    executeSql(command) {
        this.sqlite.create({
            name: 'timeregisterdata.db',
            location: 'default'
        }).then((db: SQLiteObject) => {

            db.executeSql("SELECT COUNT(*) AS ExistingTables FROM sqlite_master WHERE type='table' AND name='timeregister';", [])
                .then(data => {
                    const existingTables = data.rows.item(0).ExistingTables;
                    if (existingTables <= 0) {
                        db.executeSql('create table timeregister(time VARCHAR(25), type VARCHAR(6), processed INTEGER)', [])
                            .then(() => command(db))
                            .catch(e => console.log(e));
                    } else {
                        command(db);
                    }
                })
                .catch(err => console.log(err));
        }).catch(e => console.log(e));
    }

    insertData(type) {
        const date = new Date();
        const year = date.getFullYear().toString();

        let month = (date.getMonth() + 1).toString();
        if (month.length < 2) { month = '0' + month; }

        let day = date.getDate().toString();
        if (day.length < 2) { day = '0' + day; }

        let hour = date.getHours().toString();
        if (hour.length < 2) { hour = '0' + hour; }

        let minute = date.getMinutes().toString();
        if (minute.length < 2) { minute = '0' + minute; }

        let second = date.getSeconds().toString();
        if (second.length < 2) { second = '0' + second; }

        let dateString = `${year}-${month}-${day} `;
        dateString += `${hour}:${minute}:${second}`;

        this.showLoading();
        this.executeSql(db => {
            db.executeSql('insert into timeregister (time, type, processed) values (?, ?, ?)', [dateString, type, 0])
                .then(r => {
                    this.hideLoading();
                })
                .catch(er => {
                    this.hideLoading();
                    console.log(er);
                    alert(JSON.stringify(er));
                });
        });
    }

    start() {
        this.insertData('Start');
    }

    end() {
        this.insertData('End');
    }

    sync() {
        this.showLoading();

        this.executeSql(db => {
            db.executeSql('select * from timeregister where processed = 0', [])
                .then(data => {
                    const result = [];
                    for (let i = 0; i < data.rows.length; i++) {
                        let item = data.rows.item(i);
                        result.push({ time: item.time, type: item.type });
                    }

                    if (result.length) {
                        this.post(result, () => {
                            this.delete(db);
                            this.hideLoading();
                        });
                    } else {
                        this.hideLoading();
                    }
                })
                .catch(er => {
                    this.hideLoading();
                    console.log(er);
                    alert(JSON.stringify(er));
                });
        });
    }

    post(data, callback) {
        this.http.post(this.serverUrl, { data: data })
            .subscribe(e => {
                callback();
            }, er => {
                this.hideLoading();
                console.log(er);
                alert(JSON.stringify(er));
            });
    }

    delete(db) {
        db.executeSql('update timeregister set processed = 1 where processed = 0', [])
            .then(r => { });
    }

    showLoading() {
        this._loading = this.loadingCtrl.create({
            content: 'Wait...'
        });
        this._loading.present();
    }

    hideLoading() {
        if (this._loading) {
            this._loading.dismiss();
            this._loading = null;
        }
    }
}
