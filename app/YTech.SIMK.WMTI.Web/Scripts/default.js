/// <reference path="jquery-1.6.3-vsdoc.js" />

// Numeric only control handler
jQuery.fn.ForceNumericOnly =
function () {
    return this.each(function () {
        $(this).keydown(function (e) {
            alert(e.keyCode);
            var key = e.charCode || e.keyCode || 0;
            // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
            return (
                key == 8 ||
                key == 9 ||
                key == 46 ||
                (key >= 37 && key <= 40) ||
                (key >= 48 && key <= 57) ||
                (key >= 96 && key <= 105));
        })
    })
};

$(function () {
    $('.btn').append($('<span />').addClass('helper'));

    $.datepicker.regional['id'] = {
        closeText: 'Tutup',
        prevText: '&#x3c;mundur',
        nextText: 'maju&#x3e;',
        currentText: 'hari ini',
        monthNames: ['Januari', 'Februari', 'Maret', 'April', 'Mei', 'Juni',
		'Juli', 'Agustus', 'September', 'Oktober', 'Nopember', 'Desember'],
        monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'Mei', 'Jun',
		'Jul', 'Agust', 'Sep', 'Okt', 'Nop', 'Des'],
        dayNames: ['Minggu', 'Senin', 'Selasa', 'Rabu', 'Kamis', 'Jumat', 'Sabtu'],
        dayNamesShort: ['Min', 'Sen', 'Sel', 'Rab', 'kam', 'Jum', 'Sab'],
        dayNamesMin: ['Mg', 'Sn', 'Sl', 'Rb', 'Km', 'jm', 'Sb'],
        weekHeader: 'Mg',
        dateFormat: 'dd-M-yy',
        firstDay: 0,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    };
    $.datepicker.setDefaults($.datepicker.regional['id']);

    $.fn.autoNumeric.defaults = {/* plugin defaults */
        aNum: '0123456789', /*  allowed  numeric values */
        aNeg: '', /* allowed negative sign / character */
        aSep: '.', /* allowed thousand separator character */
        aDec: ',', /* allowed decimal separator character */
        aSign: '', /* allowed currency symbol */
        pSign: 'p', /* placement of currency sign prefix or suffix */
        mNum: 9, /* max number of numerical characters to the left of the decimal */
        mDec: 2, /* max number of decimal places */
        dGroup: 3, /* digital grouping for the thousand separator used in Format */
        mRound: 'S', /* method used for rounding */
        aPad: true/* true= always Pad decimals with zeros, false=does not pad with zeros. If the value is 1000, mDec=2 and aPad=true, the output will be 1000.00, if aPad=false the output will be 1000 (no decimals added) Special Thanks to Jonas Johansson */
    };
});
