class utils {
    static initSelect2(elem, dataUrl) {
        $(elem).select2({
            minimumInputLength: 2,
            ajax: {
                url: dataUrl,
                data: function (params) {
                    return {
                        query: params.term
                    };
                },
                processResults: function (data) {
                    return {
                        results: data
                    }
                },
            }
        });
    }

    static get(url, onOk, onError) {
        utils.makeRequest('GET', url, onOk, onError)
    }

    static makeRequest(method, url, onOk, onError) {

        var http_request;

        if (window.XMLHttpRequest) { // Mozilla, Safari,...
            http_request = new XMLHttpRequest();
            if (http_request.overrideMimeType) {
                http_request.overrideMimeType('text/xml');
                // Ver nota sobre esta linea al final
            }
        } else if (window.ActiveXObject) { // IE
            try {
                http_request = new ActiveXObject("Msxml2.XMLHTTP");
            } catch (e) {
                try {
                    http_request = new ActiveXObject("Microsoft.XMLHTTP");
                } catch (e) { }
            }
        }

        //if (!http_request) {
        //    alert('Falla :( No es posible crear una instancia XMLHTTP');
        //    return false;
        //}

        http_request.onreadystatechange = function () {
            if (http_request.readyState == 4) {
                if (http_request.status == 200) {
                    onOk(JSON.parse(http_request.response));
                } else {
                    onError();
                }
            }
        };
        http_request.open(method, url, true);
        http_request.send();

    }
}