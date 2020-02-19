// Import the config-related things
import { ConfigurationSettings } from '../app/app.config';

import { Injectable } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { ToastController } from 'ionic-angular';

import { Auth } from '../providers/auth';

import 'rxjs/add/operator/map';

@Injectable()
export class Estabelecimento {

    url: string = null;
    constructor(public http: Http, public auth: Auth, public toastCtrl: ToastController) {
        this.url = ConfigurationSettings.apiUrl;
    }

    lojas(lat, lng) {

        let headers = new Headers();
        //console.log(this.auth.claims);
        //let authToken = this.auth.claims;
        //headers.append('Authorization', 'Bearer ' + authToken);
        headers.append('Content-Type', 'application/x-www-form-urlencoded');
        let options = new RequestOptions({ headers: headers });

        return new Promise(resolve => {
            this.http.get(this.url + 'api/loja/lojasProximas?latitude=' + lat + '&longitude=' + lng, options)
                .subscribe(data => {
                    resolve(data.json());
                }, error => {
                    this.showTempToast(error.text());
                });
        });

    }

    getPrecoPorLojaProduto(lojaId, produtoId) {
        let params = {lojaId: lojaId, produtoId: produtoId};
        return new Promise(resolve => {
            this.http.get(this.url + 'api/preco', params)
            //this.http.get(this.url + 'api/preco/loja/' + lojaId + '/produto/' + produtoId, options)
                .subscribe(data => {
                    resolve(data.json());
                }, error => {
                    this.showTempToast(error.text());
                });
        });
    }

    getProdutos(lojaId) {
        return new Promise(resolve => {
            this.http.get(this.url + 'api/loja/' + lojaId + '/produtos')
                //this.http.get(this.url + 'api/preco/loja/' + lojaId + '/produto/' + produtoId, options)
                .subscribe(data => {
                    resolve(data.json());
                }, error => {
                    this.showTempToast(error.text());
                });
        });
    }

    getProdutoPorCodBarras(lojaId, codBarras) {
        return new Promise(resolve => {
            this.http.get(this.url + 'api/loja/' + lojaId + '/produtoCodBarras?codBarras=' + codBarras)
                .subscribe(data => {
                    resolve(data.json());
                }, error => {
                    this.showTempToast(error.text());
                });
        });
    }

    getLoja(id) {
        let headers = new Headers();
        let options = new RequestOptions({ headers: headers });
        return new Promise(resolve => {
        this.http.get(this.url + 'api/loja/' + id, options)
            .subscribe(data => {
                resolve(data.json());
            }, error => {
                this.showTempToast(error.text());
            });
        });
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