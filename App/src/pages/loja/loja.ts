import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { Geolocation } from '@ionic-native/geolocation';
import { ToastController, LoadingController } from 'ionic-angular';

import { ClientePage } from '../cliente/cliente';
import { ProdutoPage } from "../produto/produto";

import { Estabelecimento } from '../../providers/estabelecimento';
import { Compra } from '../../providers/compra';

@Component({
    selector: 'page-loja',
    templateUrl: 'loja.html'
})
export class LojaPage {

    lojas: any;
    loader: any;
    lat: number;
    lng: number;
    zoom: number = 12;
    markers: [{ lat: number, lng: number, nome: string }] = [{ lat: 0, lng: 0, nome: '' }];

    constructor(public navCtrl: NavController, public navParams: NavParams, public estabelecimento: Estabelecimento, private geolocation: Geolocation, private compra: Compra, public toastCtrl: ToastController, public loadingCtrl: LoadingController) {

    }

    presentLoading() {
        this.loader = this.loadingCtrl.create({ content: "Por favor aguarde...", duration: 5000 });
        this.loader.present();
    }

    getLojas() {
        this.presentLoading();
        this.estabelecimento.lojas(this.lat, this.lng).then(data => {
            if (data !== null) {
                this.lojas = data;
                for (var i = 0; i < this.lojas.length; i++) {
                    var obj = {
                        nome: this.lojas[i].Entidade.Descricao,
                        lat: parseFloat(this.lojas[i].Entidade.Endereco.Latitude),
                        lng: parseFloat(this.lojas[i].Entidade.Endereco.Longitude)
                    };
                    this.markers.push(obj);
                }
            }
            this.loader.dismiss();
        });
    }

    entrar(loja) {
        let params = {
            loja: loja
        };
        this.navCtrl.push(ProdutoPage, params);
    }

    ionViewDidEnter() {
        if (this.compra.listaAtiva) {
            this.entrar(this.compra.listaAtiva.Loja);
            return;
        }


        this.lojas = [];

        //cordova.plugins.diagnostic.isLocationAuthorized(function (enabled) {
        //    console.log("Location is " + (enabled ? "enabled" : "disabled"));
        //    if (!enabled) {
        //        cordova.plugins.diagnostic.requestLocationAuthorization(function (status) {
        //            console.log("Authorization status is now: " + status);
        //        }, function (error) {
        //            console.error(error);
        //        });
        //    }
        //}, function (error) {
        //    console.error("The following error occurred: " + error);
        //});
        

        this.geolocation.getCurrentPosition().then((resp) => {
            this.lat = resp.coords.latitude;
            this.lng = resp.coords.longitude;
            this.getLojas();
        }).catch((error) => {
            this.showTempToast(error.text ? error.text() : error);
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
