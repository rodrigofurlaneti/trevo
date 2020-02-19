function SomenteNumero(e) {
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58) || (tecla > 95 && tecla < 106)) return true;
    else {
        if (tecla === 8 || tecla === 9 || tecla === 0) return true;
        else return false;
    }
}

function formataMascara(campo, evt, formato) {
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla)) return false;
    var result = campo.value;
    var maskIdx = formato.length - 1;
    var error = false;
    var valor = campo.value;
    var posFinal = false;
    if (campo.setSelectionRange) {
        if (campo.selectionStart === valor.length) posFinal = true;
    }
    valor = valor.replace(/[^0123456789Xx]/g, "");
    for (var valIdx = valor.length - 1; valIdx >= 0 && maskIdx >= 0; --maskIdx) {
        var chr = valor.charAt(valIdx);
        var chrMask = formato.charAt(maskIdx);
        switch (chrMask) {
            case "#":
                if (!(/\d/.test(chr))) error = true;
                result = chr + result;
                --valIdx;
                break;
            case "@":
                result = chr + result;
                --valIdx;
                break;
            default:
                result = chrMask + result;
        }
    }
    campo.value = result;
    campo.style.color = error ? "red" : "";
    if (posFinal) {
        campo.selectionStart = result.length;
        campo.selectionEnd = result.length;
    }
    return result;
}

// Formata o campo valor monetário [//000,00]
function formataValorDuasCasas(campo, evt, max) {
    if (SomenteNumerosSemVirgula(evt) === false)
        return false;

    if (campo.value.length === 0 && evt !== undefined && evt !== null && evt.key !== undefined) {
        campo.value = "0,0";
        return true;
    }

    if (campo.value.length >= max)
        return false;
    //000,00
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return false;

    var vr = campo.value = filtraNumeros(filtraCampo(campo));
    if (vr.length > 0) {
        vr = parseFloat(vr.toString()).toString();
        var tam = vr.length;
        if (tam === 1)
            campo.value = "0," + vr;
        if ((tam >= 2) && (tam <= 5)) {
            campo.value = vr.substr(0, tam - 1) + "," + vr.substr(tam - 1, tam);
        }
    }
    MovimentaCursor(campo, xPos);
    return true;
}

// Formata o campo valor monetário 
function formataValor(campo, evt) {
    //1.000.000,00 
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla)) return false;
    var vr = campo.value = filtraNumeros(filtraCampo(campo));
    if (vr.length > 0) {
        vr = parseFloat(vr.toString()).toString();
        var tam = vr.length;
        if (tam === 1) campo.value = "0,0" + vr;
        if (tam === 2) campo.value = "0," + vr;
        if ((tam > 2) && (tam <= 5)) {
            campo.value = vr.substr(0, tam - 2) + "," + vr.substr(tam - 2, tam);
        }
        if ((tam >= 6) && (tam <= 8)) {
            campo.value = vr.substr(0, tam - 5) + "." + vr.substr(tam - 5, 3) + "," + vr.substr(tam - 2, tam);
        }
        if ((tam >= 9) && (tam <= 11)) {
            campo.value = vr.substr(0, tam - 8) + "." + vr.substr(tam - 8, 3) + "." + vr.substr(tam - 5, 3) + "," + vr.substr(tam - 2, tam);
        }
        if ((tam >= 12) && (tam <= 14)) {
            campo.value = vr.substr(0, tam - 11) + "." + vr.substr(tam - 11, 3) + "." + vr.substr(tam - 8, 3) + "." + vr.substr(tam - 5, 3) + "," + vr.substr(tam - 2, tam);
        }
        if ((tam >= 15) && (tam <= 18)) {
            campo.value = vr.substr(0, tam - 14) + "." + vr.substr(tam - 14, 3) + "." + vr.substr(tam - 11, 3) + "." + vr.substr(tam - 8, 3) + "." + vr.substr(tam - 5, 3) + "," + vr.substr(tam - 2, tam);
        }
    }
    MovimentaCursor(campo, xPos);
    // ReSharper disable once PossiblyUnassignedProperty
    window.__postback();
    return true;
}

// Formata o campo valor monetário 
function formataValorTresCasas(campo, evt) {
    //1.000.000,00 
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla)) return false;
    var vr = campo.value = filtraNumeros(filtraCampo(campo));
    if (vr.length > 0) {
        vr = parseFloat(vr.toString()).toString();
        var tam = vr.length;
        if (tam === 1) campo.value = "0,0" + vr;
        if (tam === 2) campo.value = "0,0" + vr;
        if ((tam > 3) && (tam <= 5)) {
            campo.value = vr.substr(0, tam - 3) + "," + vr.substr(tam - 3, tam);
        }
        if ((tam >= 6) && (tam <= 8)) {
            campo.value = vr.substr(0, tam - 5) + "." + vr.substr(tam - 5, 3) + "," + vr.substr(tam - 3, tam);
        }
        if ((tam >= 9) && (tam <= 11)) {
            campo.value = vr.substr(0, tam - 8) + "." + vr.substr(tam - 8, 3) + "." + vr.substr(tam - 5, 3) + "," + vr.substr(tam - 3, tam);
        }
        if ((tam >= 12) && (tam <= 14)) {
            campo.value = vr.substr(0, tam - 11) + "." + vr.substr(tam - 11, 3) + "." + vr.substr(tam - 8, 3) + "." + vr.substr(tam - 5, 3) + "," + vr.substr(tam - 3, tam);
        }
        if ((tam >= 15) && (tam <= 18)) {
            campo.value = vr.substr(0, tam - 14) + "." + vr.substr(tam - 14, 3) + "." + vr.substr(tam - 11, 3) + "." + vr.substr(tam - 8, 3) + "." + vr.substr(tam - 5, 3) + "," + vr.substr(tam - 3, tam);
        }
    }
    MovimentaCursor(campo, xPos);
    // ReSharper disable once PossiblyUnassignedProperty
    window.__postback();
    return true;
}

// Formata data no padrão DD/MM/YYYY 
function formataData(campo, evt) {
    var xPos = PosicaoCursor(campo); //dd/MM/yyyy 
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla)) return false;
    var vr = campo.value = filtraNumeros(filtraCampo(campo));
    var tam = vr.length;
    if (tam >= 2 && tam < 4) campo.value = vr.substr(0, 2) + "/" + vr.substr(2);
    if (tam === 4) campo.value = vr.substr(0, 2) + "/" + vr.substr(2, 2) + "/";
    if (tam > 4) campo.value = vr.substr(0, 2) + "/" + vr.substr(2, 2) + "/" + vr.substr(4);
    MovimentaCursor(campo, xPos);
    return true;
}
//descobre qual a posição do cursor no campo 
function PosicaoCursor(textarea) {
    var pos = 0;
    if (typeof (document.selection) != "undefined") { //IE 
        var range = document.selection.createRange();
        var i;
        for (i = textarea.value.length; i > 0; i--) {
            if (range.moveStart("character", 1) === 0) break;
        }
        pos = i;
    }
    if (typeof (textarea.selectionStart) != "undefined") { //FireFox 
        pos = textarea.selectionStart;
    }
    if (pos === textarea.value.length) return 0; //retorna 0 quando não precisa posicionar o elemento 
    else return pos; //posição do cursor 
}
// move o cursor para a posição pos 
function MovimentaCursor(textarea, pos) {
    if (pos <= 0) return; //se a posição for 0 não reposiciona 
    if (typeof (document.selection) != "undefined") {
        //IE 
        var oRange = textarea.createTextRange();
        oRange.moveStart("character", -textarea.value.length);
        oRange.moveEnd("character", -textarea.value.length);
        oRange.moveStart("character", pos);
        //oRange.moveEnd("character", pos); 
        oRange.select();
        textarea.focus();
    }
    if (typeof (textarea.selectionStart) != "undefined") {
        //FireFox 

        textarea.selectionStart = pos;
        textarea.selectionEnd = pos;
    }
}
//Formata data e hora no padrão DD/MM/YYYY HH:MM 
function formataDataeHora(campo, evt) {
    var xPos = PosicaoCursor(campo); //dd/MM/yyyy 
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla)) return;
    var vr = campo.value = filtraNumeros(filtraCampo(campo));
    var tam = vr.length;
    if (tam >= 2 && tam < 4) campo.value = vr.substr(0, 2) + "/" + vr.substr(2);
    if (tam === 4) campo.value = vr.substr(0, 2) + "/" + vr.substr(2, 2) + "/";
    if (tam > 4) campo.value = vr.substr(0, 2) + "/" + vr.substr(2, 2) + "/" + vr.substr(4);
    if (tam > 8 && tam < 11) campo.value = vr.substr(0, 2) + "/" + vr.substr(2, 2) + "/" + vr.substr(4, 4) + " " + vr.substr(8, 2);
    if (tam >= 11) campo.value = vr.substr(0, 2) + "/" + vr.substr(2, 2) + "/" + vr.substr(4, 4) + " " + vr.substr(8, 2) + ":" + vr.substr(10);
    campo.value = campo.value.substr(0, 16); // 
    if (xPos === 2 || xPos === 5) // 
        xPos = xPos + 1; // 
    if (xPos === 11 || xPos === 14) // 
        xPos = xPos + 2;
    MovimentaCursor(campo, xPos);
}

//Formata hora no padrão HH:MM 
function formataHoraMinuto(campo, evt) {
    var xPos = PosicaoCursor(campo); //HH:MM 
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla)) return false;
    var vr = campo.value = filtraNumeros(filtraCampo(campo));
    var tam = vr.length;
    if (tam >= 2 && tam < 4) campo.value = vr.substr(0, 2) + ":" + vr.substr(2);
    if (tam === 4) campo.value = vr.substr(0, 2) + ":" + vr.substr(2, 2);
    campo.value = campo.value.substr(0, 16); // 
    MovimentaCursor(campo, xPos);
    return true;
}

// Formata só números 
function formataInteiro(campo, evt) { //1234567890 
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla)) return false;
    campo.value = filtraNumeros(filtraCampo(campo));
    MovimentaCursor(campo, xPos);
    return true;
}

//// Formata hora no padrao HH:MM function formataHora(campo, evt) { //HH:mm 
//xPos = PosicaoCursor(campo);
//evt = getEvent(evt);
//var tecla = getKeyCode(evt);
//if (!teclaValida(tecla)) return;
//vr = campo.value = filtraNumeros(filtraCampo(campo));
//if (tam == 2) campo.value = vr.substr(0, 2) + ':';
//if (tam > 2 && tam < 5) campo.value = vr.substr(0, 2) + ':' + vr.substr(2); // if(xPos == 2) // xPos = xPos + 1; MovimentaCursor(campo, xPos); } 

// limpa todos os caracteres especiais do campo solicitado 
function filtraCampo(campo) {
    var s = "";
    var vr = campo.value;
    var tam = vr.length;
    var i;
    for (i = 0; i < tam; i++) {
        if (vr.substring(i, i + 1) !== "/" && vr.substring(i, i + 1) !== "-" && vr.substring(i, i + 1) !== "." && vr.substring(i, i + 1) !== "(" && vr.substring(i, i + 1) !== ")" && vr.substring(i, i + 1) !== ":" && vr.substring(i, i + 1) !== ",") {
            s = s + vr.substring(i, i + 1);
        }
    }
    return s;
    //return campo.value.replace("/", "").replace("-", "").replace(".", "").replace(",", "");
}
// limpa todos caracteres que não são números 
function filtraNumeros(campo) {
    var s = "";
    var vr = campo;
    var tam = vr.length;
    var i;
    for (i = 0; i < tam; i++) {
        if (vr.substring(i, i + 1) === "0" || vr.substring(i, i + 1) === "1" || vr.substring(i, i + 1) === "2" || vr.substring(i, i + 1) === "3" || vr.substring(i, i + 1) === "4" || vr.substring(i, i + 1) === "5" || vr.substring(i, i + 1) === "6" || vr.substring(i, i + 1) === "7" || vr.substring(i, i + 1) === "8" || vr.substring(i, i + 1) === "9") {
            s = s + vr.substring(i, i + 1);
        }
    }
    return s; //return campo.value.replace("/", "").replace("-", "").replace(".", "").replace(",", "") } 
    // limpa todos caracteres que não são letras 
}

function filtraCaracteres(campo) {
    var vr = campo;
    var tam = 0;
    for (var i = 0; i < tam; i++) {
        //Caracter 
        if (vr.charCodeAt(i) !== 32 && vr.charCodeAt(i) !== 39 && vr.charCodeAt(i) !== 94 && (vr.charCodeAt(i) < 65 || (vr.charCodeAt(i) > 90 && vr.charCodeAt(i) < 96) || vr.charCodeAt(i) > 122) && vr.charCodeAt(i) < 192) {
            vr = vr.replace(vr.substr(i, 1), "");
        }
    }
    return vr;
} // limpa todos caracteres que não são números, menos a vírgula 

function filtraNumerosComVirgula(campo) {
    var s = "";
    var vr = campo;
    var tam = vr.length;
    var complemento = 0;
    //flag paga contar o número de virgulas 
    for (var i = 0; i < tam; i++) {
        if ((vr.substring(i, i + 1) === "," && complemento === 0 && s !== "") || vr.substring(i, i + 1) === "0" || vr.substring(i, i + 1) === "1" || vr.substring(i, i + 1) === "2" || vr.substring(i, i + 1) === "3" || vr.substring(i, i + 1) === "4" || vr.substring(i, i + 1) === "5" || vr.substring(i, i + 1) === "6" || vr.substring(i, i + 1) === "7" || vr.substring(i, i + 1) === "8" || vr.substring(i, i + 1) === "9") {
            if (vr.substring(i, i + 1) === ",") complemento = complemento + 1;
            s = s + vr.substring(i, i + 1);
        }
    }
    return s;
}

function formataMesAno(campo, evt) {
    //MM/yyyy
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla)) return;
    var vr = campo.value = filtraNumeros(filtraCampo(campo));
    var tam = vr.length;
    if (tam >= 2) campo.value = vr.substr(0, 2) + "/" + vr.substr(2);
    MovimentaCursor(campo, xPos);
}

function formataCNPJ(campo, evt) {
    //99.999.999/9999-99 
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla)) return;
    var vr = campo.value = filtraNumeros(filtraCampo(campo));
    var tam = vr.length;
    if (tam >= 2 && tam < 5) campo.value = vr.substr(0, 2) + "." + vr.substr(2);
    else if (tam >= 5 && tam < 8) campo.value = vr.substr(0, 2) + "." + vr.substr(2, 3) + "." + vr.substr(5);
    else if (tam >= 8 && tam < 12) campo.value = vr.substr(0, 2) + "." + vr.substr(2, 3) + "." + vr.substr(5, 3) + "/" + vr.substr(8);
    else if (tam >= 12) campo.value = vr.substr(0, 2) + "." + vr.substr(2, 3) + "." + vr.substr(5, 3) + "/" + vr.substr(8, 4) + "-" + vr.substr(12);
    MovimentaCursor(campo, xPos);
}

function formataCPF(campo, evt) {
    //999.999.999-99 
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla)) return;
    var vr = campo.value = filtraNumeros(filtraCampo(campo));
    var tam = vr.length;
    if (tam >= 3 && tam < 6) campo.value = vr.substr(0, 3) + "." + vr.substr(3);
    else if (tam >= 6 && tam < 9) campo.value = vr.substr(0, 3) + "." + vr.substr(3, 3) + "." + vr.substr(6);
    else if (tam >= 9) campo.value = vr.substr(0, 3) + "." + vr.substr(3, 3) + "." + vr.substr(6, 3) + "-" + vr.substr(9);
    MovimentaCursor(campo, xPos);
}

function formataDouble(campo, evt) {
    //18,53012 
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla)) return;
    campo.value = filtraNumerosComVirgula(campo.value);
    MovimentaCursor(campo, xPos);
}

function formataTelefone(campo, evt) {
    //(00) 0000-0000 
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla)) return;
    var vr = campo.value = filtraNumeros(filtraCampo(campo));
    var tam = vr.length;
    if (tam === 1) campo.value = "(" + vr;
    else if (tam >= 2 && tam < 6) campo.value = "(" + vr.substr(0, 2) + ") " + vr.substr(2);
    else if (tam >= 6) campo.value = "(" + vr.substr(0, 2) + ") " + vr.substr(2, 4) + "-" + vr.substr(6);
    //( // 
    if (xPos === 1 || xPos === 3 || xPos === 5 || xPos === 9) // 
        xPos = xPos + 1;
    MovimentaCursor(campo, xPos);
}

function formataTexto(campo, evt, sMascara) {
    //Nome com Inicial Maiuscula. 
    evt = getEvent(evt);
    var xPos = PosicaoCursor(campo);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla)) return false;
    campo.value = filtraCaracteres(filtraCampo(campo));
    var valor;
    var count;
    var i;
    var pos;
    var valorIni;
    var valorMei;
    var valorFim;
    if (sMascara === "Aa" || sMascara === "Xx") {
        valor = campo.value.toLowerCase();
        count = campo.value.split(" ").length - 1;
        pos = 0;
        valor = valor.substring(0, 1).toUpperCase() + valor.substring(1, valor.length);
        for (i = 0; i < count; i++) {
            pos = valor.indexOf(" ", pos + 1);
            valorIni = valor.substring(0, valor.indexOf(" ", pos - 1)) + " ";
            valorMei = valor.substring(valor.indexOf(" ", pos) + 1, valor.indexOf(" ", pos) + 2).toUpperCase();
            valorFim = valor.substring(valor.indexOf(" ", pos) + 2, valor.length);
            valor = valorIni + valorMei + valorFim;
        }
        campo.value = valor;
    }

    if (sMascara === "Aaa" || sMascara === "Xxx") {
        valor = campo.value.toLowerCase();
        count = campo.value.split(" ").length - 1;
        pos = 0;
        var ligacao = false;
        var chrLigacao = Array("de", "da", "das", "do", "dos", "para", "e");
        valor = valor.substring(0, 1).toUpperCase() + valor.substring(1, valor.length);
        for (i = 0; i < count; i++) {
            ligacao = false;
            pos = valor.indexOf(" ", pos + 1);
            valorIni = valor.substring(0, valor.indexOf(" ", pos - 1)) + " ";
            for (var a = 0; a < chrLigacao.length; a++) {
                if (valor.substring(valorIni.length, valor.indexOf(" ", valorIni.length)).toLowerCase() === chrLigacao[a].toLowerCase()) {
                    ligacao = true;
                    break;
                } else if (ligacao === false && valor.indexOf(" ", valorIni.length) === -1) {
                    if (valor.substring(valorIni.length, valor.length).toLowerCase() === chrLigacao[a].toLowerCase()) {
                        ligacao = true;
                        break;
                    }
                }
            }
            if (ligacao === true) {
                valorMei = valor.substring(valor.indexOf(" ", pos) + 1, valor.indexOf(" ", pos) + 2).toLowerCase();
            } else {
                valorMei = valor.substring(valor.indexOf(" ", pos) + 1, valor.indexOf(" ", pos) + 2).toUpperCase();
            }
            valorFim = valor.substring(valor.indexOf(" ", pos) + 2, valor.length);
            valor = valorIni + valorMei + valorFim;
        }
        campo.value = valor;
    }
    MovimentaCursor(campo, xPos);
    return true;
}

// Formata o campo CEP 
function formataCEP(campo, evt) {
    //312555-650 
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla)) return;
    var vr = campo.value = filtraNumeros(filtraCampo(campo));
    var tam = vr.length;
    if (tam < 5) campo.value = vr;
    else if (tam === 5) campo.value = vr + "-";
    else if (tam > 5) campo.value = vr.substr(0, 5) + "-" + vr.substr(5);
    MovimentaCursor(campo, xPos);
}

function formataCartaoCredito(campo, evt) {
    //0000.0000.0000.0000 
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla)) return;
    var vr = campo.value = filtraNumeros(filtraCampo(campo));
    var tammax = 16;
    var tam = vr.length;
    if (tam < tammax && tecla !== 8) {
        tam = vr.length + 1;
    }
    if (tam < 5) {
        campo.value = vr;
    }
    if ((tam > 4) && (tam < 9)) {
        campo.value = vr.substr(0, 4) + "." + vr.substr(4, tam - 4);
    }
    if ((tam > 8) && (tam < 13)) {
        campo.value = vr.substr(0, 4) + "." + vr.substr(4, 4) + "." + vr.substr(8, tam - 4);
    }
    if (tam > 12) {
        campo.value = vr.substr(0, 4) + "." + vr.substr(4, 4) + "." + vr.substr(8, 4) + "." + vr.substr(12, tam - 4);
    }
    MovimentaCursor(campo, xPos);
}

//recupera tecla 
//evita criar mascara quando as teclas são pressionadas 
function teclaValida(tecla) {
    if (tecla === 8
        //backspace 
        //Esta evitando o post, quando são pressionadas estas teclas. 
        //Foi comentado pois, se for utilizado o evento texchange, é necessario o post. 
        || tecla === 9 //TAB 
        || tecla === 27 //ESC 
        || tecla === 16 //Shif TAB 
        || tecla === 45 //insert 
        || tecla === 46 //delete 
        || tecla === 35 //home 
        || tecla === 36 //end 
        || tecla === 37 //esquerda 
        || tecla === 38 //cima 
        || tecla === 39 //direita 
        || tecla === 40) //baixo 
        return false;
    else return true;
}

// recupera o evento do form 
function getEvent(evt) {
    if (!evt) evt = window.event;
    //IE 
    return evt;
}

//Recupera o código da tecla que foi pressionado 
function getKeyCode(evt) {
    var code;
    if (typeof (evt.keyCode) == "number") code = evt.keyCode;
    else if (typeof (evt.which) == "number") code = evt.which;
    else if (typeof (evt.charCode) == "number") code = evt.charCode;
    else return 0;
    return code;
}

function onMaskMoney(element) {
    var el = element
        , exec = function (v) {
            v = v.replace(/\D/g, "");
            v = new String(Number(v));
            var len = v.length;
            if (1 === len)
                v = v.replace(/(\d)/, "0.00$1");
            else if (2 === len)
                v = v.replace(/(\d)/, "0.0$1");
            else if (len > 3) {
                v = v.replace(/(\d{3})$/, ".$1");
            }
            else if (len > 2) {
                v = v.replace(/(\d{3})$/, "0.$1");
            }
            //else if (len > 2) {
            //    v = v.replace(/(\d{2})$/,".$1");
            //}
            return v;
        };

    setTimeout(function () {
        el.value = exec(el.value).replace(".", ",");
    }, 1);
}

function SomenteNumeros(e) {
    var tecla = (window.event) ? e.keyCode : e.which;
    if (tecla === 8 || tecla === 0)
        return true;
    if (tecla !== 44 && tecla < 48 || tecla > 57)
        return false;
    return true;
}

function SomenteNumerosSemVirgula(e) {
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla === 8 || tecla === 0) return true;
        else return false;
    }
}

function MascaraMoeda(objTextBox, SeparadorMilesimo, SeparadorDecimal, e) {
    var sep = 0;
    var key = '';
    var i = j = 0;
    var len = len2 = 0;
    var strCheck = '0123456789';
    var aux = aux2 = '';
    var whichCode = (window.Event) ? e.which : e.keyCode;
    if (whichCode == 13) return true;
    key = String.fromCharCode(whichCode); // Valor para o código da Chave
    if (strCheck.indexOf(key) == -1) return false; // Chave inválida
    len = objTextBox.value.length;
    for (i = 0; i < len; i++)
        if ((objTextBox.value.charAt(i) != '0') && (objTextBox.value.charAt(i) != SeparadorDecimal)) break;
    aux = '';
    for (; i < len; i++)
        if (strCheck.indexOf(objTextBox.value.charAt(i)) != -1) aux += objTextBox.value.charAt(i);
    aux += key;
    len = aux.length;
    if (len == 0) objTextBox.value = '';
    if (len == 1) objTextBox.value = '0' + SeparadorDecimal + '0' + aux;
    if (len == 2) objTextBox.value = '0' + SeparadorDecimal + aux;
    if (len > 2) {
        aux2 = '';
        for (j = 0, i = len - 3; i >= 0; i--) {
            if (j == 3) {
                aux2 += SeparadorMilesimo;
                j = 0;
            }
            aux2 += aux.charAt(i);
            j++;
        }
        objTextBox.value = '';
        len2 = aux2.length;
        for (i = len2 - 1; i >= 0; i--)
            objTextBox.value += aux2.charAt(i);
        objTextBox.value += SeparadorDecimal + aux.substr(len - 2, len);
    }
    return false;
}

function isValidDate(dateString) {
    // First check for the pattern
    if (!/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(dateString))
        return false;

    // Parse the date parts to integers
    var parts = dateString.split("/");
    var day = parseInt(parts[0], 10);
    var month = parseInt(parts[1], 10);
    var year = parseInt(parts[2], 10);

    // Check the ranges of month and year
    if (year < 1000 || year > 3000 || month == 0 || month > 12)
        return false;

    var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    // Adjust for leap years
    if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
        monthLength[1] = 29;

    // Check the range of the day
    return day > 0 && day <= monthLength[month - 1];
}

function validaNumeros(evt) {
    var theEvent = evt || window.event;

    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    var regex = /[0-9]/;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

function somenteNumeros(campo) {
    var initVal = $(campo).val();
    outputVal = initVal.replace(/[^0-9]/g, "");
    if (initVal !== outputVal) {
        $(campo).val(outputVal);
    }
}

function validarPeriodo(dataInicial, dataFinal, dataPodemSerIguais) {

    if (!dataInicial || !dataFinal)
        return false;

    if (!dataPodemSerIguais && dataInicial === dataFinal)
        return false;

    if (dataInicial > dataFinal)
        return false;

    return true;
}



function verificarCheckBoxesMarcados(chekboxlista) {

    var aChk = document.getElementsByName(chekboxlista);

    for (var i = 0; i < aChk.length; i++) {

        if (aChk[i].checked === true) {

            return true;
        }
    }

    return false;
}


function BuscarColunasTotal(column = 1) {
    let result = 0;
    let columns = $("#datatable_fixed_column tr td:nth-child(" + column + ")");

    columns.each(i => {

        var valor = $(columns[i]).html();

        valor = valor.replace('R$', '');

        valor = valor.replace(',', '.');


        result += parseFloat(valor);
    });

    return result;
}


function BuscarColunasQtdeTotal(column = 1) {
    let result = 0;
    let columns = $("#datatable_fixed_column tr td:nth-child(" + column + ")");

    columns.each(i => {

        var valor = $(columns[i]).html();
        result += parseInt(valor);
    });

    return result;
}


function BuscarQtdeLinhas() {

    var result = $("#datatable_fixed_column tr").length - 2;

    return result;
}

function limpaCombo(id) {
    $(id).empty();
    $(id).append('<option value="">Selecione...</option>');
    $(id).trigger("chosen:updated");
}

function populaCombo(idCombo, controller, action, filtro, criarSelecione = 1, callback = undefined) {
    limpaCombo(idCombo);

    if (!filtro)
        filtro = {};

    $.ajax({
        url: `/${controller}/${action}`,
        type: "POST",
        dataType: "json",
        data: filtro,
        success: function (result) {

            $(idCombo).empty();

            if (criarSelecione !== 0)
                $(idCombo).append('<option value="">Selecione...</option>');

            for (let index = 0; index < result.length; index++) {
                let item = result[index];
                $(idCombo).append(`<option value='${item.Id}'>${item.Descricao}</option>`);
            }

            $(idCombo).trigger("chosen:updated");
        },
        error: function (error) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();

            if (callback !== undefined)
                callback();
        }
    });
}

function dataMenorQueDataAtual(data, adicionarDias = 0) {
    let d = new Date();
    d.setDate(d.getDate() + adicionarDias);

    let dia = d.getDate();
    let mes = d.getMonth() + 1;
    if (mes < 10)
        mes = "0" + mes;
    let ano = d.getFullYear();
    let dataAtual = parseInt(ano + "" + mes + "" + dia);
    let dataSelecionada = parseInt(data.split("/").reverse().join(""));

    return dataSelecionada < dataAtual;
}

function generateUUID() { // Public Domain/MIT
    var d = new Date().getTime();//Timestamp
    var d2 = (performance && performance.now && (performance.now() * 1000)) || 0;//Time in microseconds since page-load or 0 if unsupported
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16;//random number between 0 and 16
        if (d > 0) {//Use timestamp until depleted
            r = (d + r) % 16 | 0;
            d = Math.floor(d / 16);
        } else {//Use microseconds since page-load if supported
            r = (d2 + r) % 16 | 0;
            d2 = Math.floor(d2 / 16);
        }
        return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
}