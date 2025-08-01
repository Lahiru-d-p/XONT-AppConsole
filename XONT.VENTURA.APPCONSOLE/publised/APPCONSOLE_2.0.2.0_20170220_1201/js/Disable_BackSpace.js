﻿
$(document).unbind('keydown').bind('keydown', function (event) {
    var doPrevent = false;
    if (event.keyCode === 8) {
        var d = event.srcElement || event.target;
        if ((d.tagName.toUpperCase() === 'INPUT' &&
             (
                 d.type.toUpperCase() === 'TEXT' ||
                 d.type.toUpperCase() === 'PASSWORD' ||
                 d.type.toUpperCase() === 'FILE' ||
                 d.type.toUpperCase() === 'SEARCH' ||
                 d.type.toUpperCase() === 'EMAIL' ||
                 d.type.toUpperCase() === 'NUMBER' ||
                 d.type.toUpperCase() === 'DATE')
             ) ||
             d.tagName.toUpperCase() === 'TEXTAREA') {
            doPrevent = d.readOnly || d.disabled;
        }
        else if ((d.tagName.toUpperCase() === 'INPUT' &&
             (
                 d.type.toUpperCase() === 'RADIO' ||
                 d.type.toUpperCase() === 'CHECKBOX'
               
             )))
        {
            doPrevent = true;
        }
        else {
            doPrevent = true;
        }
    }

    if (doPrevent) {
        event.preventDefault();
    }
});

