
$(document).ready(function () {
    if (obj) {
        $('#formCadastro #Nome').val(obj.Nome);
        $('#formCadastro #CEP').val(obj.CEP);
        $('#formCadastro #Email').val(obj.Email);
        $('#formCadastro #Sobrenome').val(obj.Sobrenome);
        $('#formCadastro #Nacionalidade').val(obj.Nacionalidade);
        $('#formCadastro #Estado').val(obj.Estado);
        $('#formCadastro #Cidade').val(obj.Cidade);
        $('#formCadastro #Logradouro').val(obj.Logradouro);
        $('#formCadastro #Telefone').val(obj.Telefone);
        $('#formCadastro #CPF').val(obj.CPF);
    }

    $('#formCadastro').submit(function (e) {
        e.preventDefault();
        
        $.ajax({
            url: urlPost,
            method: "POST",
            data: {
                "NOME": $(this).find("#Nome").val(),
                "CEP": $(this).find("#CEP").val(),
                "Email": $(this).find("#Email").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Telefone": $(this).find("#Telefone").val(),
                "CPF": $(this).find("#CPF").val(),
            },
            error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
            success:
                function (r) {
                CadastroBeneficiariosNovos(r.Id);
            }
        });
    })

    $('#formBeneficiario').submit(function (e) {
        e.preventDefault();

        let listaObjeto = [{ Id: 0, IdCliente: 0, Nome: $(this).find("#NomeBeneficiario").val(), Cpf: $(this).find("#CPFBeneficiario").val(), }];

        let nomes = document.querySelectorAll(".novoNome");
        let cpfs = document.querySelectorAll(".novoCPF");

        let nomesVelhos = document.querySelectorAll(".nomeVelho");
        let cpfsVelhos = document.querySelectorAll(".cpfVelho");

        for (let i = 0; i < nomes.length; i++) {
            listaObjeto.push({ Id: 0, IdCliente: 0, Nome: nomes[i].innerHTML, CPF: cpfs[i].innerHTML, })
        }

        for (let i = 0; i < nomesVelhos.length; i++) {
            listaObjeto.push({ Id: 0, IdCliente: 0, Nome: nomesVelhos[i].innerHTML, CPF: cpfsVelhos[i].innerHTML, })
        }

        listaObjeto = JSON.stringify(listaObjeto);

        $('#cpfBeneficiario').val("");
        $('#nomeBeneficiario').val("");

        $.ajax({
            url: urlBeneficiarioPost,
            contentType: 'application/json; charset=utf-8',
            method: "POST",
            data: listaObjeto,
            error:
                function (r) {
                    if (r.status == 400)
                        ModalDialog("Ocorreu um erro", r.responseJSON);
                    else if (r.status == 500)
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                },
            success:
                function (r) {
                    AdicionarLinhaTabela(r.Nome, r.CPF);

                }
        });
    })


})

function AdicionarLinhaTabela(nome, cpf) {
        var texto = '<tr>' +
        '<td class="CPF novoCPF">'+ cpf +'</td >' +
        '<td class="novoNome">' + nome + '</td>' +
        '<td> <button type="button" class="btn btn-primary">Alterar</button> ' +
        ' <button type="button" class="btn btn-primary" > Excluir </button> </td> ' +
        '</tr>';
    $('#inicioTabela').prepend(texto);
    
}

function CadastroBeneficiariosNovos(id) {
    let nomes = document.querySelectorAll(".novoNome");
    let cpfs = document.querySelectorAll(".novoCPF");

    let listaObjeto = [];
    for (let i = 0; i < nomes.length; i++) {
        listaObjeto.push({ Id: 0, IdCliente: id, Nome: nomes[i].innerHTML, CPF: cpfs[i].innerHTML, })
    }

    listaObjeto = JSON.stringify(listaObjeto);

    $.ajax({
        url: urlInserirBeneficiarioPost,
        contentType: 'application/json; charset=utf-8',
        method: "POST",
        data: listaObjeto,
        error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
        success:
            function (r) {
                ModalDialog("Sucesso!", "cadastro realizado");

            }
    });
}

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}
