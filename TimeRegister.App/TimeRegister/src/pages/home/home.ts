import { Component } from '@angular/core';
import { NavController, LoadingController } from 'ionic-angular';
import { SQLite, SQLiteObject } from '@ionic-native/sqlite';
import { Http } from '@angular/http';
import { RegisterService } from '../../services/register-service/register-service';

@Component({
    selector: 'page-home',
    templateUrl: 'home.html'
})
export class HomePage {

    _loading: any;
    serverUrl: string = "http://10.0.0.244:5000/v1/time";

    constructor(public navCtrl: NavController, private loadingCtrl: LoadingController, private registerService: RegisterService) { }

    start(){
        this.showLoading();
        this.registerService.start(() => {
            this.hideLoading();
        });
    }

    end(){
        this.showLoading();
        this.registerService.end(() => {
            this.hideLoading();
        });
    }

    sync(){
        this.showLoading();
        this.registerService.sync(this.serverUrl,() => {
            this.hideLoading();
        });
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
