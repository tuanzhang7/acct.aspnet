function getUrlParam(name) {
    return decodeURI(
        (RegExp(name + '=' + '(.+?)(&|$)').exec(location.search) || [, null])[1]
            );
}
function DisplayMessage(display, input, css, text) {
    if ($('#message_div').length) {

    }
    else {
        $("body").append('<div id="message_div" class="ajaxLoading"><span>Loading......</span></div>');
    }
    var message = $('#message_div');
    // clear all old css
    //message.removeClass("ajaxLoading");

    if (display) {
        if (css == "input-error") {
            message.data("error", text);
            $(input).attr("error", "true");
        }

        message.text(text);
        message.addClass(css);
    } else {
        // remove the attribute the error isn't 
        // being displayed any more
        if (css == "input-error") {
            message.removeData("error");
            $(input).removeAttr("error");
        }

        // if the error attribute is still present then display
        // the error message else clear out the text
        if (message.data("error") != undefined && message.data("error") != "") {
            message.text(message.data("error"));
            message.addClass("input-error");
        } else {
            message.text("");
        }
    }
}

function DisplayError(display, input, message) {
    DisplayMessage(display, input, "input-error", message);
}
function ShowError(message, autoHide) {
    if ($('#messageDiv').length) {
        $('#messageDiv').removeClass('ajaxLoading').addClass('ajaxMsgError');

    }
    else {
        $("body").append('<div id="messageDiv" class="ajaxMsgError"><span></span></div>');
    }
    $("#messageDiv").html(message);
    $("#messageDiv").show();
    if (autoHide) {
        $("#messageDiv").delay(5000).fadeOut(800, function () {
        });
    }
}
function ShowMessage(message, autoHide) {
    if ($('#messageDiv').length) {
        $('#messageDiv').removeClass('ajaxMsgError').addClass('ajaxLoading');
    }
    else {
        $("body").append('<div id="messageDiv" class="ajaxLoading"><span></span></div>');
    }
    $("#messageDiv").html(message);
    $("#messageDiv").show();
    if (autoHide) {
        $("#messageDiv").delay(5000).fadeOut(800, function () {
        });
    }
}
function HideMessage() {
    $("#messageDiv").hide();
}

function LoadingMessage(message) {
    if ($('#loadMsgDiv').length) {
        //console.debug('loadMsgDiv exist');
    }
    else {
        $("body").append('<div id="loadMsgDiv" class="ajaxLoading"><span></span></div>');
    }
    $("#loadMsgDiv").html(message);
    $("#loadMsgDiv").show();
}
function HideLoadingMessage() {
    if ($('#loadMsgDiv').length) {
        $("#loadMsgDiv").hide();
    }
}

var timeout = null;
function ajaxStart() {
    //console.debug('ajaxStart');
    if (timeout) {
        clearTimeout(timeout);
    }
    else {
        timeout = setTimeout(function () { LoadingMessage('Loading......') }, 500);
        //console.debug('ajaxStart' + timeout);
    }
}
function ajaxStop() {
    //console.debug('ajaxStop' + timeout);
    if (timeout) {
        clearTimeout(timeout);
        timeout = null;
    }
    HideLoadingMessage()
}
function DisplayErrors(errors) {
    for (var i = 0; i < errors.length; i++) {
        $("<label for='" + errors[i].Key + "' class='error'></label>").html(errors[i].Value[0]).appendTo($("#" + errors[i].Key).parent());
    }
}
//Typeahead
function initTypeahead(remote) {
    var bestPictures = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: remote
    });

    bestPictures.initialize();

    

    $('#q').typeahead(
    {
        hint: true,
        highlight: true,
        minLength: 2
    },
    {
        name: 'value',
        displayKey: 'value',
        source: bestPictures.ttAdapter(),
        templates: {
            suggestion: Handlebars.compile([
              '<p class="repo-language"><a href="/Customer/Details/6">{{value}}</a> </p>'
            ].join(''))
        }
    });
}
