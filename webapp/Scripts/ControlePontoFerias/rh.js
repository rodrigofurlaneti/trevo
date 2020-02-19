function AtualizarDadosTotais() {
    return get("AtualizarDadosTotais")
        .done((response) => {
            $("#total-intervalo-pendente").val(response.IntervalosPendentes);
            $("#total-he-sessenta-cinco").val(response.TotalHoraExtraSessentaCinco);
            $("#total-asn").val(response.TotalAdicionalNoturno);
            $("#total-falta").val(response.TotalFalta);
            $("#total-atrasos").val(response.TotalAtraso);
            $("#total-he-cem").val(response.TotalHoraExtraCem);
            $("#total-feriado").val(response.TotalFeriadosTrabalhados);
        });
}