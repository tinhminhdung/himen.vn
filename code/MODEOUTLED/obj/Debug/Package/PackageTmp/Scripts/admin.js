function ShowLoadingIndicator() {
    if (typeof(disableLoadingIndicator) != 'undefined' && disableLoadingIndicator) {
	    return;
    }
    var windowWidth = $j(window).width();
    var scrollTop;
    if(self.pageYOffset) {
	    scrollTop = self.pageYOffset;
    }
    else if(document.documentElement && document.documentElement.scrollTop) {
	    scrollTop = document.documentElement.scrollTop;
    }
    else if(document.body) {
	    scrollTop = document.body.scrollTop;
    }
    $j('#AjaxLoading').css('position', 'absolute');
    $j('#AjaxLoading').css('top', scrollTop+'px');
    $j('#AjaxLoading').css('left', parseInt((windowWidth-150)/2)+"px");
    $j('#AjaxLoading').show();
}

function HideLoadingIndicator() {
    //$j('#AjaxLoading').hide();
    $j('#AjaxLoading').delay(800).slideUp(300);
}
//Check nhập vào là số và dấu phân cách
$j(document).ready(function() {
    $j('html').ajaxStart(function() {
	    ShowLoadingIndicator();
    });

    $j('html').ajaxComplete(function() {
	    HideLoadingIndicator();
    });
    $j('.InitialFocus').focus();

    $j(window).scroll(function () {
        var offset = $j(".widget-title").offset();
        var curWin = $j(window);
        var top = offset.top - curWin.scrollTop();
        var bottom = $j(window).height() - $j(".widget-title").height();
        bottom = bottom - offset.top;

        var compare = bottom - curWin.scrollTop();
        if (top <= 0) {
            if ($j(".scrollBox").hasClass("hidden")) {
                $j(".scrollBox").removeClass("hidden");

            }
        } else {
            if (!$j(".scrollBox").hasClass("hidden")) {
                $j(".scrollBox").addClass("hidden");
            }

        }
    });
});

var separator = ",";  // use comma as 000's separator
var decpoint = ".";  // use period as decimal point
var percent = "%";
var currency = "$";  // use dollar sign for currency

function replaceAll(str, from, to) {
    var idx = str.indexOf(from);
    while (idx > -1) {
        str = str.replace(from, to);
        idx = str.indexOf(from);
    }
    return str;
}

function remote_format(input) {
    var output = '';
    //output=replaceAll(input,'.','');
    output = replaceAll(input, ",", "");
    return output;
}

function strip(input, chars) {  // strip all characters in 'chars' from input
    var output = "";  // initialise output string
    for (var i = 0; i < input.length; i++)
        if (chars.indexOf(input.charAt(i)) == -1)
            output += input.charAt(i);
    return output;
}

function separate(input, separator) {  // format input using 'separator' to mark 000's
    input = "" + input;
    var output = "";  // initialise output string
    for (var i = 0; i < input.length; i++) {
        if (i != 0 && (input.length - i) % 3 == 0) output += separator;
        output += input.charAt(i);
    }
    return output;
}

function formatNumber(number, format, print) {  // use: formatNumber(number, "format")
    //number=remote_format(number);
    if (print) document.write("formatNumber(" + number + ", \"" + format + "\")<br>");

    if (number - 0 != number) return null;  // if number is NaN return null
    var useSeparator = format.indexOf(separator) != -1;  // use separators in number
    var usePercent = format.indexOf(percent) != -1;  // convert output to percentage
    var useCurrency = format.indexOf(currency) != -1;  // use currency format
    var isNegative = (number < 0);
    number = Math.abs(number);
    if (usePercent) number *= 100;
    format = strip(format, separator + percent + currency);  // remove key characters
    number = "" + number;  // convert number input to string

    // split input value into LHS and RHS using decpoint as divider
    var dec = number.indexOf(decpoint) != -1;
    var nleftEnd = (dec) ? number.substring(0, number.indexOf(".")) : number;
    var nrightEnd = (dec) ? number.substring(number.indexOf(".") + 1) : "";

    // split format string into LHS and RHS using decpoint as divider
    dec = format.indexOf(decpoint) != -1;
    var sleftEnd = (dec) ? format.substring(0, format.indexOf(".")) : format;
    var srightEnd = (dec) ? format.substring(format.indexOf(".") + 1) : "";

    // adjust decimal places by cropping or adding zeros to LHS of number
    if (srightEnd.length < nrightEnd.length) {
        var nextChar = nrightEnd.charAt(srightEnd.length) - 0;
        nrightEnd = nrightEnd.substring(0, srightEnd.length);
        if (nextChar >= 5) nrightEnd = "" + ((nrightEnd - 0) + 1);  // round up

        // patch provided by Patti Marcoux 1999/08/06
        while (srightEnd.length > nrightEnd.length) {
            nrightEnd = "0" + nrightEnd;
        }

        if (srightEnd.length < nrightEnd.length) {
            nrightEnd = nrightEnd.substring(1);
            nleftEnd = (nleftEnd - 0) + 1;
        }
    } else {
        for (var i = nrightEnd.length; srightEnd.length > nrightEnd.length; i++) {
            if (srightEnd.charAt(i) == "0") nrightEnd += "0";  // append zero to RHS of number
            else break;
        }
    }

    // adjust leading zeros
    sleftEnd = strip(sleftEnd, "#");  // remove hashes from LHS of format
    while (sleftEnd.length > nleftEnd.length) {
        nleftEnd = "0" + nleftEnd;  // prepend zero to LHS of number
    }

    if (useSeparator) nleftEnd = separate(nleftEnd, separator);  // add separator
    var output = nleftEnd + ((nrightEnd != "") ? "." + nrightEnd : "");  // combine parts
    output = ((useCurrency) ? currency : "") + output + ((usePercent) ? percent : "");
    if (isNegative) {
        // patch suggested by Tom Denn 25/4/2001
        output = (useCurrency) ? "(" + output + ")" : "-" + output;
    }
    return output;
}