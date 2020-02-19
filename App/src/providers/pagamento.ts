// Import the config-related things
import { ConfigurationSettings } from '../app/app.config';

import { Injectable } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';

import { Auth } from '../providers/auth';

import 'rxjs/add/operator/map';

@Injectable()
export class Pagamento {
    url: string = null;

    constructor(public http: Http, public auth: Auth, public alertCtrl: AlertController, public toastCtrl : ToastController) {
        this.url = ConfigurationSettings.apiUrl;
    }

    //getCartao() {

    //    let headers = new Headers();
    //    console.log(this.auth.claims);
    //    let authToken = this.auth.claims.access_token;
    //    headers.append('Authorization', 'Bearer ' + authToken);
    //    headers.append('Content-Type', 'application/json');
    //    let options = new RequestOptions({ headers: headers });

    //    return new Promise(resolve => {
    //        this.http.get(this.url + 'api/cartao', options)
    //            .subscribe(data => {
    //                console.log(data.json())
    //                resolve(data.json());
    //            }, error => {
    //                console.log("Error: Cartão");
    //            });
    //    });

    //}

    salvar(cartao) {

        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        console.log(this.auth.claims);
        let authToken = this.auth.claims;
        headers.append('Authorization', 'Bearer ' + authToken.access_token);
        let options = new RequestOptions({ headers: headers });
        return new Promise(resolve => {
            this.http.post(this.url + 'api/cartao', cartao, options)
                .subscribe(data => {
                    resolve(data.json());
                }, error => {
                    this.showTempToast(error.text());
                });
        });

    }

    remover(cartao) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        let authToken = this.auth.claims;
        headers.append('Authorization', 'Bearer ' + authToken.access_token);
        let options = new RequestOptions({ headers: headers });
        return new Promise(resolve => {
            this.http.post(this.url + 'api/cartao/deletar/' + cartao.Id, {}, options)
                .subscribe(data => {
                    resolve(data.json());
                }, error => {
                    console.log("Error: Remover Cartão");
                    this.showTempToast(error.text());
                });
        });
    }

    update(cartao) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        let authToken = this.auth.claims;
        headers.append('Authorization', 'Bearer ' + authToken.access_token);
        let options = new RequestOptions({ headers: headers });
        return new Promise(resolve => {
            this.http.post(this.url + 'api/cartao/' + cartao.Id, cartao, options)
                .subscribe(data => {
                    resolve(data.json());
                }
                , error => {
                    console.log("Error: Salvar Cartão");
                    this.showTempToast(error.text());
                }
                );
        });
    }

    showErrorAlert(message) {
        let alert = this.alertCtrl.create({
            title: 'Um erro ocorreu.',
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
