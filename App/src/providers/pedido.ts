// Import the config-related things
import { ConfigurationSettings } from '../app/app.config';

import { Injectable } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';

import { Auth } from '../providers/auth';

import 'rxjs/add/operator/map';

@Injectable()
export class Pedido {

    url: string = null;

    constructor(public http: Http, public alertCtrl: AlertController, public auth: Auth, public toastCtrl: ToastController) {
        this.url = ConfigurationSettings.apiUrl;
    }

    finalizaPedido(pedido) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        let authToken = this.auth.claims;
        headers.append('Authorization', 'Bearer ' + authToken.access_token);
        let options = new RequestOptions({ headers: headers });
        return new Promise(resolve => {
            this.http.post(this.url + 'api/pedido/finalizar', pedido, options)
                .subscribe(data => {
                        resolve(data.json());
                    },
                    error => {
                        this.showTempToast(error.text());
                    });
        });
    }

    getPedidos() {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        let authToken = this.auth.claims;
        headers.append('Authorization', 'Bearer ' + authToken.access_token);
        let options = new RequestOptions({ headers: headers });
        return new Promise(resolve => {
            this.http.get(this.url + 'api/pedido/', options)
                .subscribe(data => {
                    let json = data.json();
                    resolve(json);
                },
                error => {
                    this.showTempToast(error.text());
                });
        });
    }

    showAlert(title, subtitle) {
        let alert = this.alertCtrl.create({
            title: title,
            subTitle: subtitle,
            buttons: ['OK']
        });
        alert.present();
    }

    showErrorAlert(message) {
        let alert = this.alertCtrl.create({
            title: 'Um erro ocorreu.',
            subTitle: message,
            buttons: ['OK']
        });
        alert.present();
    }

    showSuccessAlert(message) {
        let alert = this.alertCtrl.create({
            title: 'Sucesso',
            subTitle: message,
            buttons: ['OK']
        });
        alert.present();
    }

    showTempToast(message) {
        let text = "";

        if (/^[\],:{}\s]*$/.test(message.replace(/\\["\\\/bfnrtu]/g, '@')
            .replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']')
            .replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {
            var json = JSON.parse(message);
            if (json.isTrusted) {
                text = "Um erro de conexão ocorreu. Tente novamente.";
            } else {
                text = message;
            }
        } else {
            text = message;
        }

        const toast = this.toastCtrl.create({
            message: text,
            duration: 4000,
            position: 'bottom',
            //showCloseButton: true,
            //closeButtonText: 'Ok'
        });
        //toast.onDidDismiss(this.dismissHandler);
        toast.present();
    }
}