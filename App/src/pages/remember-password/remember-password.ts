import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';

import { LoadingController } from 'ionic-angular';

import { Auth } from '../../providers/auth';

@Component({
    selector: 'page-remember-password',
    templateUrl: 'remember-password.html'
})
export class RememberPasswordPage {

    loader: any;

    usercreds = {
        name: ''
    };

    constructor(public navCtrl: NavController, public navParams: NavParams, public loadingCtrl: LoadingController, public auth: Auth) { }

    changePassword(user) {

        this.presentLoading();

        console.log('called remember-password');
        this.usercreds = user;
        console.log('usercreds.name: ' + this.usercreds.name);

        this.auth.rememberPassword(this.usercreds).then(data => {

            if (data !== null) {
                //this.navCtrl.setRoot();
                console.log(data);
            }

        });

        this.loader.dismiss();
    }

    presentLoading() {

        this.loader = this.loadingCtrl.create({ content: "Por favor aguarde..." });
        this.loader.present();

    }

    ionViewDidLoad() {
        console.log('ionViewDidLoad RememberPasswordPage');
    }

}
