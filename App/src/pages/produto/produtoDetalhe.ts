import { Component } from '@angular/core';
import { NavController, NavParams, AlertController } from 'ionic-angular';

import { ListaCompraPage } from "../listaCompra/listaCompra";

import { Estabelecimento } from '../../providers/estabelecimento';
import { Compra } from '../../providers/compra';

/*
  Generated class for the Produto page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
    selector: 'page-produto',
    templateUrl: 'produtoDetalhe.html'
})
export class ProdutoDetalhePage {

    produtoMv: any;
    loja: any;
    preco: any;

    constructor(public navCtrl: NavController, public navParams: NavParams, public estabelecimento: Estabelecimento, public alertCtrl : AlertController, public compra : Compra) {
        this.produtoMv = navParams.get("produto");
        this.loja = navParams.get("loja");
    }

    ionViewDidLoad() {
        console.log('ionViewDidLoad ProdutoPage');
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

    showAlert(title, subtitle) {
        let alert = this.alertCtrl.create({
            title: title,
            subTitle: subtitle,
            buttons: ['OK']
        });
        alert.present();
    }

    visualizarListaDeCompra() {
        this.navCtrl.setRoot(ListaCompraPage);
    }
}