import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { AlertController } from 'ionic-angular';
import { ToastController } from 'ionic-angular';
import { BarcodeScanner, BarcodeScannerOptions } from '@ionic-native/barcode-scanner';

import { ListaCompraPage } from "../listaCompra/listaCompra";
import { ProdutoDetalhePage } from "../produto/produtoDetalhe";
import { LojaPage } from "../loja/loja";

import { Estabelecimento } from '../../providers/estabelecimento';
import { Compra } from '../../providers/compra';
/*
  Generated class for the Produto page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
    selector: 'page-produto',
    templateUrl: 'produto.html'
})
export class ProdutoPage {

    loja: any;
    qtde: any;
    produtos: any;
    nomeLoja: any;
    barcodeOptions: BarcodeScannerOptions;

    constructor(public navCtrl: NavController, public navParams: NavParams, public alertCtrl: AlertController, public toastCtrl: ToastController, public estabelecimento: Estabelecimento, public compra: Compra, private barcodeScanner: BarcodeScanner) {
        this.loja = this.navParams.get("loja");

        this.estabelecimento.getProdutos(this.loja.Id).then(data => {
            if (data !== null) {
                return this.produtos = data;
            }
        });

        this.compra.getListaTemporaria(this.loja.Id);

        this.estabelecimento.getLoja(this.loja.Id).then(data => {
            this.nomeLoja = data["Descricao"];
        });
    }

    ionViewDidLoad() {

    }

    detalhes(produtoMv) {
        let params = {
            loja: this.loja,
            produto: produtoMv
        };
        this.navCtrl.push(ProdutoDetalhePage, params);
    }

    decrementaQtde(produtoMv) {
        if (produtoMv.Quantidade <= 1)
            return;

        produtoMv.Quantidade--;
    }

    incrementaQtde(produtoMv) {
        produtoMv.Quantidade++;
    }

    adicionarCarrinho(produtoMv) {
        this.compra.adicionarItem(produtoMv);
    }

    visualizarListaDeCompra() {
        this.navCtrl.setRoot(ListaCompraPage);
    }

    escolherLojas() {
        this.compra.listaAtiva = null;
        this.navCtrl.setRoot(LojaPage);
    }

    showAlert(title, subtitle) {
        let alert = this.alertCtrl.create({
            title: title,
            subTitle: subtitle,
            buttons: ['OK']
        });
        alert.present();
    }

    showBarcodeScan() {

        this.barcodeOptions = {
            prompt: 'Escaneie um produto para buscá-lo na loja.'
        };

        this.barcodeScanner.scan(this.barcodeOptions).then((barcodeData) => {

            if (barcodeData.cancelled) {
                this.showTempToast("Escaneamento cancelado.");
                return;
            }

            let text = barcodeData.text;
            this.estabelecimento.getProdutoPorCodBarras(this.loja.Id, text).then(produtoMv => {
                if (produtoMv != null) {
                    this.detalhes(produtoMv);
                } else {
                    this.showTempToast("Este produto não foi encontrado na loja.");
                }
            });

        }, (err) => {
            this.showTempToast(err);
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