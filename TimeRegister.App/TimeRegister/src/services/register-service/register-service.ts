import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { SQLite, SQLiteObject } from '@ionic-native/sqlite';

@Injectable()
export class RegisterService {

  constructor(private http: Http, private sqlite: SQLite) { }

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

  insertData(type, callback) {
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

    this.executeSql(db => {
      db.executeSql('insert into timeregister (time, type, processed) values (?, ?, ?)', [dateString, type, 0])
        .then(r => {
          callback();
        })
        .catch(er => {
          callback();
          console.log(er);
          alert(JSON.stringify(er));
        });
    });
  }

  start(callback) {
    this.insertData('Start', callback);
  }

  end(callback) {
    this.insertData('End', callback);
  }

  sync(serverUrl, callback) {
    this.executeSql(db => {
      db.executeSql('select * from timeregister where processed = 0', [])
        .then(data => {
          const result = [];
          for (let i = 0; i < data.rows.length; i++) {
            let item = data.rows.item(i);
            result.push({ time: item.time, type: item.type });
          }

          if (result.length) {
            this.post(serverUrl, result, () => {
              this.delete(db);
              callback();
            });
          } else {
            callback();
          }
        })
        .catch(er => {
          callback();
          console.log(er);
          alert(JSON.stringify(er));
        });
    });
  }

  post(serverUrl, data, callback) {
    this.http.post(serverUrl, { data: data })
      .subscribe(e => {
        callback();
      }, er => {
        callback();
        console.log(er);
        alert(JSON.stringify(er));
      });
  }

  delete(db) {
    db.executeSql('update timeregister set processed = 1 where processed = 0', [])
      .then(r => { });
  }
}
