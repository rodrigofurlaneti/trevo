import { Component, ViewChild } from '@angular/core';
import { Nav, Platform } from 'ionic-angular';
import { StatusBar, Splashscreen } from 'ionic-native';

import { LojaPage } from '../pages/loja/loja';
import { LoginPage } from '../pages/login/login';
import { ClientePage } from '../pages/cliente/cliente';
import { CartaoPage } from '../pages/cartao/cartao';
import { ListaCompraPage } from '../pages/listaCompra/listaCompra';
import { ListaPedidoPage } from '../pages/pedido/listaPedido';

import { StorageCtrl } from "../providers/storageCtrl";
import { Auth } from '../providers/auth';

@Component({
    templateUrl: 'app.html'
})
export class MyApp {
    @ViewChild(Nav) nav: Nav;

    rootPage: any = LojaPage;

    pages: Array<{ title: string, icon: string, component: any }>;

    constructor(public platform: Platform, public storage: StorageCtrl, public auth: Auth) {
        this.initializeApp();

        let login = { title: 'Login', icon: "log-in", component: LoginPage };
        let loja = { title: 'Loja', icon: "home", component: LojaPage };
        let listaCompras = { title: 'Lista de compra', icon: "cart", component: ListaCompraPage };
        let dadosPessoais = { title: 'Dados Pessoais', icon: "contact", component: ClientePage };
        let cartao = { title: 'Cartões', icon: "card", component: CartaoPage };
        let pedidos = { title: 'Pedidos', icon: "filing", component: ListaPedidoPage };

        this.pages = [login, loja, listaCompras];

        this.auth.userLogged.subscribe(claims => {
            if (this.auth.claims) {
                this.pages = [loja, dadosPessoais, cartao, listaCompras, pedidos];
            }
        });
    }

    initializeApp() {
        this.platform.ready().then(() => {
            // Okay, so the platform is ready and our plugins are available.
            // Here you can do any higher level native things you might need.
            StatusBar.styleDefault();
            Splashscreen.hide();
            this.storage.clear();
        });
    }

    openPage(page) {
        // Reset the content nav to have just this page
        // we wouldn't want the back button to show in this scenario
        this.nav.setRoot(page.component);
    }
}
