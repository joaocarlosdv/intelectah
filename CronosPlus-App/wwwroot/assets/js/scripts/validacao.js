/*  -----validarForm-----
    Tipos de data-validacao:
        inputObrigatoria, dataObrigatoria, emailObrigatorio, inputUsername
        cpfObrigatorio, cnpjObrigatorio, senhaUsuario
*/
async function validarForm(idForm, callback) {

    // Remover espaços em branco em todos os campos
    $('input, textarea').each(function () {
        if (this.type === "file") return;

        const val = $(this).val().trim()
        $(this).val(val)
    })

    // Limpa o helpBlock quando o usuário começar a digitar um valor
    $('.money').on('keyup', limparHelpBlock);
    
    // sempre que o usuário entrar no campo obrigatório não preenhido, a mensagem de erro será apagada
    $('input, textarea, select').on('input', limparHelpBlock);

    validationUtils.limparAlertas();

    const formulario = document.getElementById(idForm);
    const elementos = validationUtils.escanearElementos(idForm, [
        "input",
        "textarea",
        "select",
    ]);
    const id = $("input[name=Id]").length ? $("input[name=Id]").val() : null;

    const validacao = await efetuarValidacao(elementos, id);
    //console.log(elementos)
    //console.log(validacao)

    const valido = validacao.every((v) => v !== false);

    if (!valido) {
        return false;
    }

    if (typeof callback === "function") {
        return callback();
    }

    // Pega todos os elementos que possuem a classe money
    const elementosComMoney = elementos.filter(function (elemento) {
        return $(elemento).hasClass('money');
    });

    // Remove o símbolo de moeda e espaços em branco nos elementos com a classe .money
    elementosComMoney.forEach(function (elemento) {
        const valorCampo = $(elemento).val();
        const valorNumerico = valorCampo.replace('R$', '').replace(/\s/g, '');
        $(elemento).val(valorNumerico);
    });


    // Pega todos os elementos que possuem a classe numericoComMascara
    const elementosnumericoComMascara = elementos.filter(function (elemento) {
        return $(elemento).hasClass('numericoComMascara');
    });

    // Remove todos os caracteres e espaços em branco nos elementos com a classe .numericoComMascara
    elementosnumericoComMascara.forEach(function (elemento) {
        const valorCampoMascara = $(elemento).val();
        const valorNumericoSemMascara = valorCampoMascara.replace(/\D/g, '');
        $(elemento).val(valorNumericoSemMascara);
    });


    // Pega todos os elementos que possuem a classe top-input e atribui maiúsculo
    const elementosTopInput = elementos.filter(function (elemento) {
        return $(elemento).hasClass('top-input');
    });

    // Transforma todos os elementos da classe .top-input em maiúsculo e remove os espaços em branco (exceto email)
    elementosTopInput.forEach(function (elemento) {
        const fieldType = $(elemento).attr('type');
        const fieldName = $(elemento).attr('name');
        const fieldId = $(elemento).attr('id');

        if (
            fieldType !== 'email' &&
            (fieldName === undefined || fieldName.indexOf('email') === -1) &&
            (fieldId === undefined || fieldId.indexOf('email') === -1)
        )
        {
            let valorElemento = $(elemento).val();
            if (valorElemento) {
                valorElemento = $(elemento).val().trim().toUpperCase(); // retira os espaços e transforma em maiúsculo
                valorElemento = valorElemento.normalize("NFD").replace(/[\u0300-\u036f]/g, ""); // retira os acentos
                $(elemento).val(valorElemento);
            }
        }
    });

    loadAnimacao(true);

    $(formulario).submit();
    $(formulario).removeAttr('method')
    $(formulario).removeAttr('action')
}
function limparHelpBlock(event) {
    var campoId = event.target.id;
    var helpBlockId = 'hb' + campoId;

    // Limpa o conteúdo do help-block específico
    $('#' + helpBlockId).html('');
}
async function efetuarValidacao(elementos, id) {
    return await Promise.all(
        elementos.map(async (e) => {
            if ($(e).is(":checkbox")) return;

            const elemento = $(e)[0];

            if (elemento.disabled) return;
            

            const verificarDisponibilidade = elemento.getAttribute(
                "data-disponibilidade"
            )
                ? elemento.getAttribute("data-disponibilidade")
                : false;
            const tipoValidacao = elemento.getAttribute("data-validacao")
                ? elemento.getAttribute("data-validacao").split("|")[0]
                : null;
            const tipoElemento = elemento.getAttribute("data-validacao")
                ? elemento.getAttribute("data-validacao").split("|")[1]
                : null;

            const camposValidos = await validation.validate(
                elemento,
                tipoValidacao,
                tipoElemento,
                id
            );

            if (camposValidos && verificarDisponibilidade)
                return (
                    camposValidos && (await validation.validateAvailability(elemento, id))
                );

            return camposValidos;
        })
    );
}

async function validarFormPorArray(listaDeElementos, callback = null) {
    validationUtils.limparAlertas();
    var elementos = new Array();
    listaDeElementos.map((el) => {
        elementos.push(validationUtils.pegarElemento(el));
    });

    elementos = elementos.filter(function (element) {
        return element !== undefined;
    });

    const id = $("input[name=Id]").length ? $("input[name=Id]").val() : null;

    const validacao = await efetuarValidacao(elementos, id);
    const valido = validacao.every((v) => v !== false);

    if (!valido) return false;

    if (typeof callback === "function") return callback();

    return true;
}

const validationUtils = {
    emitirAlerta: (elemento, mensagemErro = "") => {
        let elementoNome = "";
        if (typeof elemento != "string") {
            elementoNome = $(elemento).attr("name");
        } else {
            elementoNome = elemento;
        }

        $(".help-block").map(function () {
            const helpBlock = $(this).attr("data-help-for") === elementoNome;
            if (helpBlock) {
                $(this).html(mensagemErro);
            }
        });
    },
    escanearElementos: (idForm, tipoElementos) => {
        const elementos = [];
        tipoElementos.map((tipoElemento) => {
            $(`form#${idForm} ${tipoElemento}`).each(function () {
                const el = $(this);
                elementos.push(el);
            });
        });
        return elementos;
    },
    pegarElemento: (nome) => {
        return $(`[name="${nome}"]`)[0];
    },
    checarDisponibilidade: async (elemento, id) => {
        const endpoint = elemento
            .getAttribute("data-disponibilidade")
            .split("|")[0];
        const key = elemento.getAttribute("data-disponibilidade").split("|")[1];
        const val = $(elemento).val();
        const res = await $.post(endpoint, { [`${key}`]: val, id: id }, function (res) {
            return res.responseJSON;
        });

        if (res) {
            return {
                disponivel: true,
                mensagem: "",
            };
        } else {
            return {
                disponivel: false,
                mensagemErro: `<p>Este <strong>${key}</strong> já está sendo utilizado</p>`,
            };
        }
    },
    validarCheckboxes: (length) => {
        if (length === 0) return;
        const valid = checkboxes.some((c) => c);
        if (valid) {
            const mensagemErro = "";
            this.emitirAlerta(elemento, tipoValidacao, mensagemErro);
            return true;
        } else {
            const mensagemErro =
                "<p>Selecione no mínimo um <strong>" + tipoElemento + "</strong>.</p>";
            return this.emitirAlerta(elemento, tipoValidacao, mensagemErro);
        }
    },
    limparAlertas: () => {
        $(".help-block").html("");
    },
    validarCaracteresEspeciais: (event) => {
        var k = event.keyCode;
        return ((k > 64 && k < 91) || (k > 96 && k < 123) || k === 8 || (k >= 48 && k <= 57));
    },
};

const validationRules = {
    campoVazio: function (value) {        
        return value && value.length === 0;
    },
    espacoEmBranco: function (value) {
        return !value.replace(/\s/gim, '').length
        //return value.indexOf(" ") >= 0;
    },
    isValidUsername: function (value) {
        var regx = /^[A-Za-z0-9_]{6,25}$/;
        return regx.test(value);
    },
    validarSenha: function (senha, confirmacaoDeSenha) {
        if (senha == confirmacaoDeSenha) return true;
        return false;
    },
    validarEmail: function (email) {
        var er = /^[a-zA-Z0-9][a-zA-Z0-9\._-]+@([a-zA-Z0-9\._-]+\.)[a-zA-Z-0-9]{2}/;
        if (!er.exec(email)) {
            return false;
        }
        return true;
    },

    validarLoginAdmDisponibilidade: async function (loginAdm) {
        var obj = null;
        await $.ajax({
            url: `/Pessoa/getByLoginAdm?loginAdm=${loginAdm}`,
            timeout: 5000,
            success: function (res) {
                obj = res;
            },
            async: false
        });
        return obj;
    },
    validarCpf: function (cpf) {
        cpf = cpf.replace(".", "").replace(".", "").replace("-", "");
        var numeros, digitos, soma, i, resultado, digitos_iguais;
        digitos_iguais = 1;
        if (cpf.length < 11) return false;
        for (i = 0; i < cpf.length - 1; i++) {
            if (cpf.charAt(i) != cpf.charAt(i + 1)) {
                digitos_iguais = 0;
                break;
            }
        }
        if (!digitos_iguais) {
            numeros = cpf.substring(0, 9);
            digitos = cpf.substring(9);
            soma = 0;
            for (i = 10; i > 1; i--) {
                soma += numeros.charAt(10 - i) * i;
            }
            resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11);
            if (resultado != digitos.charAt(0)) {
                return false;
            }
            numeros = cpf.substring(0, 10);
            soma = 0;
            for (i = 11; i > 1; i--) {
                soma += numeros.charAt(11 - i) * i;
            }
            resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11);
            if (resultado != digitos.charAt(1)) {
                return false;
            }
            return true;
        } else {
            return false;
        }
    },
        
    validarCNPJ: function (cnpj) {
        var i = 0;
        var l = 0;
        var strNum = "";
        var strMul = "6543298765432";
        var character = "";
        var iValido = 1;
        var iSoma = 0;
        var strNum_base = "";
        var iLenNum_base = 0;
        var iLenMul = 0;
        var iSoma = 0;
        var strNum_base = 0;
        var iLenNum_base = 0;

        if (cnpj == "") return false;
        cnpj = cnpj
            .replace(".", "")
            .replace(".", "")
            .replace("/", "")
            .replace("-", "");
        l = cnpj.length;
        for (i = 0; i < l; i++) {
            caracter = cnpj.substring(i, i + 1);
            if (caracter >= "0" && caracter <= "9") strNum = strNum + caracter;
        }

        if (strNum.length != 14) return false;

        strNum_base = strNum.substring(0, 12);
        iLenNum_base = strNum_base.length - 1;
        iLenMul = strMul.length - 1;
        for (i = 0; i < 12; i++)
            iSoma =
                iSoma +
                parseInt(
                    strNum_base.substring(iLenNum_base - i, iLenNum_base - i + 1),
                    10
                ) *
                parseInt(strMul.substring(iLenMul - i, iLenMul - i + 1), 10);

        iSoma = 11 - (iSoma - Math.floor(iSoma / 11) * 11);
        if (iSoma == 11 || iSoma == 10) iSoma = 0;

        strNum_base = strNum_base + iSoma;
        iSoma = 0;
        iLenNum_base = strNum_base.length - 1;
        for (i = 0; i < 13; i++)
            iSoma =
                iSoma +
                parseInt(
                    strNum_base.substring(iLenNum_base - i, iLenNum_base - i + 1),
                    10
                ) *
                parseInt(strMul.substring(iLenMul - i, iLenMul - i + 1), 10);

        iSoma = 11 - (iSoma - Math.floor(iSoma / 11) * 11);
        if (iSoma == 11 || iSoma == 10) iSoma = 0;
        strNum_base = strNum_base + iSoma;
        if (strNum != strNum_base) return false;

        return true;
    },
    validarData: function (dataString) {
        var dataArray = dataString.split("/");
        var dia = parseInt(dataArray[0]);
        var mes = parseInt(dataArray[1]) - 1;
        var ano = parseInt(dataArray[2]);

        currentDate = new Date();
        var dataReferencia = new Date(ano, mes, dia);

        if (
            dataReferencia.getDate() != dia ||
            dataReferencia.getMonth() != mes ||
            dataReferencia.getFullYear() != ano ||
            dataReferencia.getFullYear() > currentDate.getFullYear() ||
            dataReferencia.getFullYear() < 1900
        ) {
            return false;
        }
        return true;
    },
    
    validarTelefone: function (telefone) {
        return telefone.length >= 10;
    },
    validarCep: function (value) {
        return (value.length = 8);
    },
    validarTamanhoMinimo: function (element, tamanhoMinimo = 5) {
        return element.length >= tamanhoMinimo;
    },
    validarUnique: async function (value, url, paiId) {
        var id = 0;
        var idstring = paiId != undefined ? '&id=' + paiId : '';
        await $.ajax({
            url: url + `?nome=${value}${idstring}`,
            success: function (res) {
                id = res;
            },
            async: false
        })
        return id;
    },
    isZero: value => {
        const newValue = value.replace(/\./gim, '').replace(/,/gim, '.').replace(/\D/g, '')
        const test = Math.ceil(newValue) === 0
        return test
    },
    possuiMenssagemCustomizada: function (elemento) {
        const mensagemCustomizada = $(elemento).data('msg-customizada');
        return !!mensagemCustomizada
    }
};

const validation = {
    validate: async function (elemento, tipoValidacao, tipoElemento = "", id = 0) {
        switch (tipoValidacao) {
            case "telefoneObrigatorio":
                if (!validationRules.validarTelefone(elemento.value)) {
                    const mensagemErro =
                        "<p>O campo <strong> Telefone </strong> é obrigatório.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                }
                validationUtils.emitirAlerta(elemento);
                return true;
            case "inputUsername":
                if (elemento.value.length < 1) {
                    const mensagemErro = `<p>Utilize ao menos <strong>1 caracter</strong></p>`;
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                } else if (validationRules.espacoEmBranco(elemento.value)) {
                    const mensagemErro = `<p>Não utilize apenas <strong>espaços em branco</strong></p>`;
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                }
                validationUtils.emitirAlerta(elemento);
                return true;
            case "inputObrigatoria":
                if (elemento.value == "" || elemento.value.length == 0) {                    
                    mensagemErro = "<p>O campo <strong>" +
                        tipoElemento +
                        "</strong> é obrigatório.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                } 
                return true;
            case "inputObrigatoriaM":
                if (elemento.value == "" || elemento.value.length == 0) {
                    mensagemErro = "<p>Informe o <strong>" +
                        tipoElemento +
                        "</strong>.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                }
                return true;
            case "inputObrigatoriaF":
                if (elemento.value == "" || elemento.value.length == 0) {
                    mensagemErro = "<p>Informe a <strong>" +
                        tipoElemento +
                        "</strong>.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                }
                return true;

            case "loginAdmUnico":
                if (validationRules.campoVazio(elemento.value)) {
                    const mensagemErro =
                        "<p>Informe o <strong>" +
                        tipoElemento +
                        "</strong>.</p>";
                    return validationUtils.emitirAlerta(elemento, mensagemErro);
                }

                let pessoaLoginAdm = await validationRules.validarLoginAdmDisponibilidade(elemento.value);
                if (pessoaLoginAdm !== null && pessoaLoginAdm.id > 0) {
                    modalConfirmacao({
                        titulo: 'Login já registrado',
                        texto: `Oops! Este login já está registrado. Por favor tente informar outro.`,
                        labelCancelar: `Entendi`,
                        esconderBotaoConfirmar: true,
                    })
                    $('#modalConfirmacao').modal('show');

                    const mensagemErro =
                        "<p><strong>" +
                        tipoElemento +
                        "</strong> inválido.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);

                    return false;
                }
                return true;

            case "dataObrigatoria":                
                if (validationRules.campoVazio(elemento.value)) {
                    const mensagemErro =
                        "<p>O campo <strong>" +
                        tipoElemento +
                        "</strong> é obrigatório.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                } else {
                    if (!validationRules.validarData(elemento.value)) {
                        const mensagemErro =
                            "<p>O campo <strong>" +
                            tipoElemento +
                            "</strong> deve ser preenchido com uma data válida.</p>";
                        validationUtils.emitirAlerta(elemento, mensagemErro);
                        return false;
                    }
                }
                validationUtils.emitirAlerta(elemento);
                return true;
            case "emailObrigatorio":
                if (validationRules.campoVazio(elemento.value)) {
                    const mensagemErro =
                        "<p>O campo <strong>" +
                        tipoElemento +
                        "</strong> é obrigatório.</p>";
                    return validationUtils.emitirAlerta(elemento, mensagemErro);
                } else {
                    if (!validationRules.validarEmail(elemento.value)) {
                        const mensagemErro =
                            "<p>O campo <strong>" +
                            tipoElemento +
                            "</strong> deve ser preenchido com um E-Mail válido.</p>";
                        validationUtils.emitirAlerta(elemento, mensagemErro);
                        return false;
                    }
                }
                validationUtils.emitirAlerta(elemento);
                return true;
            case "emailOpcional":
                if (elemento.value != '' && !validationRules.validarEmail(elemento.value)) {
                    const mensagemErro =
                        "<p>O campo <strong>" +
                        tipoElemento +
                        "</strong> deve ser preenchido com um E-Mail válido.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                }
                return true;

            case "cpfCnpjObrigatorio":
                let valor = elemento.value;
                valor = valor.replace(/\D/g, '');

                if (validationRules.campoVazio(valor) || valor == "" || valor.length == 0) {
                    const mensagemErro = "<p>O campo <strong>CPF/CNPJ</strong> é obrigatório.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                } else if (valor.length <= 11 && !validationRules.validarCpf(valor)) {
                    const mensagemErro = "<p>O campo deve ser preenchido com um <strong>CPF</strong> válido.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                } else if (valor.length > 11 && valor.length <= 14 && !validationRules.validarCNPJ(valor)) {
                    const mensagemErro = "<p>O campo deve ser preenchido com um <strong>CNPJ</strong> válido.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                }

                
                validationUtils.emitirAlerta(elemento);
                return true;

            case "cpfObrigatorio":
                if (validationRules.campoVazio(elemento.value)) {
                    const mensagemErro =
                        "<p>O campo <strong>" +
                        tipoElemento +
                        "</strong> é obrigatório.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                } else {
                    if (!validationRules.validarCpf(elemento.value)) {
                        const mensagemErro =
                            "<p>O campo <strong>" +
                            tipoElemento +
                            "</strong> deve ser preenchido com um CPF válido.</p>";
                        validationUtils.emitirAlerta(elemento, mensagemErro);
                        return false;
                    }
                }
                validationUtils.emitirAlerta(elemento);
                return true;

            case "cnpjObrigatorio": {
                if (!validationRules.validarCNPJ(elemento.value)) {
                    const mensagemErro =
                        "<p>O campo <strong>" +
                        tipoElemento +
                        "</strong> deve ser preenchido com um CNPJ válido.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                }
                return true;
            }

            case "RazaoSocialObrigatorio": {
                
                if (elemento.value == "" || elemento.value.length == 0) {                    
                    const mensagemErro =
                        "<p>O campo <strong>" +
                        tipoElemento +
                        "</strong> deve ser preenchido.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                } else {
                    validationUtils.limparAlertas();
                }
                return true;
            }

            case "SetorObrigatorio": {
                if (elemento.value == -1) {
                    const mensagemErro =
                        "<p>O campo <strong>Setores</strong> deve ser selecionado.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                } else {
                    validationUtils.limparAlertas();
                }
                return true;
            }

            case "cnpjObrigatorioUnico": {
                if (!validationRules.validarCNPJ(elemento.value)) {
                    const mensagemErro =
                        "<p>O campo <strong>" +
                        tipoElemento +
                        "</strong> deve ser preenchido com um CNPJ válido.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                }

                return true;
            }
            case "senhaUsuario":
                const senha = $("#senha").val();
                const confirmacaoDeSenha = $("#confirmeSenha").val();
                if (elemento.name == "senha") {
                    if (senha == "" || senha.length == 0) {
                        mensagemErro = "<p>Informe a <strong> Senha</strong>.</p>";
                        validationUtils.emitirAlerta(elemento, mensagemErro);
                        return false;
                    }
                }
                if (elemento.name == "confirmeSenha") {
                    if (confirmacaoDeSenha == "" || confirmacaoDeSenha.length == 0) {
                        mensagemErro = "<p>Confirme a <strong> Senha</strong>.</p>";
                        validationUtils.emitirAlerta(elemento, mensagemErro);
                        return false;
                    }
                }
                if (!validationRules.validarSenha(senha, confirmacaoDeSenha)) {
                    const mensagemErro =
                        "<p><strong>Senhas</strong> não conferem.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                }
                validationUtils.emitirAlerta(elemento);
                return true;

            case "senhaColaborador":
                const senhaColaborador = $("#senhaColaborador").val();
                const confirmacaoDeSenhaColaborador = $("#confirmeSenha").val();
                if (elemento.name == "senhaColaborador") {
                    if (senhaColaborador == "" || senhaColaborador.length == 0) {
                        mensagemErro = "<p>Informe a <strong> Senha</strong>.</p>";
                        validationUtils.emitirAlerta(elemento, mensagemErro);
                        return false;
                    }
                }
                if (elemento.name == "confirmeSenha") {
                    if (confirmacaoDeSenhaColaborador == "" || confirmacaoDeSenhaColaborador.length == 0) {
                        mensagemErro = "<p>Confirme a <strong> Senha</strong>.</p>";
                        validationUtils.emitirAlerta(elemento, mensagemErro);
                        return false;
                    }
                }
                if (!validationRules.validarSenha(senhaColaborador, confirmacaoDeSenhaColaborador)) {
                    const mensagemErro =
                        "<p><strong>Senhas</strong> não conferem.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                }
                validationUtils.emitirAlerta(elemento);
                return true;

            case "selectObrigatorio":
                if (+elemento.value === -1) {
                    const mensagemErro =
                        "<p>O campo <strong>" +
                        tipoElemento +
                        "</strong> deve ser selecionado.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro, elemento.name);
                    return false;
                }
                validationUtils.emitirAlerta(elemento);
                return true;

            case "uniqueObrigatorio":
                var url = elemento.getAttribute("data-url");
                var paiId = elemento.getAttribute("data-pai-id");
                var msg = elemento.getAttribute("data-mensagem");
                var verifyId = elemento.getAttribute("data-field-id");

                if (validationRules.campoVazio(elemento.value) || validationRules.espacoEmBranco(elemento.value)) {
                    const mensagemErro =
                        "<p>O Campo <strong>" +
                        tipoElemento +
                        "</strong> é obrigatório.</p> ";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                }

                exists = await validationRules.validarUnique(elemento.value, url, paiId);
                if (exists != 0 && exists != id && exists != verifyId) {
                    const mensagemErro = msg != undefined && msg != '' ? msg :
                        "<p>Não é permitido repetir o campo <strong>" +
                        tipoElemento +
                        "</strong>. " + tipoElemento + " " + elemento.value + " já existe. </p> ";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                }

                validationUtils.emitirAlerta(elemento);
                return true;

            case "unique":
                var url = elemento.getAttribute("data-url");
                var paiId = elemento.getAttribute("data-pai-id");
                exists = await validationRules.validarUnique(elemento.value, url, paiId)
                if (exists != 0 && exists != id) {
                    const mensagemErro =
                        "<p>Não é permitido repetir o <strong>" +
                        tipoElemento +
                        "</strong>. Valor</p> " + elemento.value + " já existe.";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                }

                validationUtils.emitirAlerta(elemento);
                return true;

            case "valorObrigatorio":                
                if (validationRules.isZero(elemento.value)) {
                    const mensagemErro =
                        "<p>O campo <strong>" +
                        tipoElemento +
                        "</strong> não pode ser zero.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                } else if (validationRules.espacoEmBranco(elemento.value)) {
                    const mensagemErro =
                        "<p>O campo <strong>" +
                        tipoElemento +
                        "</strong> não pode ser vazio.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                }

                return true;
            case "valorObrigatorioRelated":
                //console.log(elemento.getAttribute('data-related'))
                //console.log(validationRules.isZero(elemento.value))
                //console.log($('input[name="' + elemento.getAttribute('data-related') + '"]').val());
                //console.log(validationRules.isZero($('input[name="' + elemento.getAttribute('data-related') + '"]').val()))
                if (validationRules.isZero(elemento.value) && validationRules.isZero($('input[name="' + elemento.getAttribute('data-related') + '"]').val())) {
                    const mensagemErro =
                        "<p>O campo <strong>" +
                        tipoElemento +
                        "</strong> não pode ser zero.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                } else if (validationRules.espacoEmBranco(elemento.value)) {
                    const mensagemErro =
                        "<p>O campo <strong>" +
                        tipoElemento +
                        "</strong> não pode ser vazio.</p>";
                    validationUtils.emitirAlerta(elemento, mensagemErro);
                    return false;
                }

                return true;
        }
        return true;
    },
    validateAvailability: async function (elemento, id) {
        const disponibilidade = await validationUtils.checarDisponibilidade(
            elemento,
            id
        );
        if (!disponibilidade.disponivel)
            validationUtils.emitirAlerta(elemento, disponibilidade.mensagemErro);

        return disponibilidade.disponivel;
    },
};
