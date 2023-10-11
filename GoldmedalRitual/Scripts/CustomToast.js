function toast(type, message, time) {
    var TimeSecond = time !== "" && typeof (time) === undefined ? time * 1000 : 3.5 * 1000;

    if (type === 's') {
        SuccessToast(message, time)
    }
    if (type === 'e') {
        ErrorToast(message, time)
    }
    if (type === 'w') {
        WarningToast(message, time)
    }
    if (type === 'i') {
        InformationToast(message, time)
    }
}

function SuccessToast(Text, time) {
    var ToastHidingTime = false;
    if (time !== "") {
        ToastHidingTime = true;
        var TimeSecond = time * 1000;
    }
    $.toast({
        text: Text, // Text that is to be shown in the toast
        heading: "Success", // Optional heading to be shown on the toast
        icon: "success", // Type of toast icon
        showHideTransition: "slide", // fade, slide or plain
        allowToastClose: true, // Boolean value true or false
        hideAfter: ToastHidingTime === true ? TimeSecond : TostHidingTime, // false to make it sticky or number representing the miliseconds as time after which toast needs to be hidden
        stack: 3, // false if there should be only one toast at a time or a number representing the maximum number of toasts to be shown at a time
        position: "top-center", // bottom-left or bottom-right or bottom-center or top-left or top-right or top-center or mid-center or an object representing the left, right, top, bottom values
        //position: {     //Custom Positioning of Toast
        //    top: 120,
        //    left: 10
        //},
        textAlign: "left", // Text alignment i.e. left, right or center
        loader: true, // Whether to show loader or not. True by default
        loaderBg: "#026103", // Background color of the toast loader
        beforeShow: function () { }, // will be triggered before the toast is shown
        afterShown: function () { }, // will be triggered after the toat has been shown
        beforeHide: function () { }, // will be triggered before the toast gets hidden
        afterHidden: function () { }, // will be triggered after the toast has been hidden
    });
}


function WarningToast(Text, time) {
    var ToastHidingTime = false;
    if (time !== "") {
        ToastHidingTime = true;
        var TimeSecond = time * 1000;
    }
    $.toast({
        text: Text,
        heading: 'Warning',
        icon: 'warning',
        showHideTransition: 'slide',
        allowToastClose: true,
        hideAfter: ToastHidingTime === true ? TimeSecond : TostHidingTime,
        stack: 3,
        position: "top-center", // bottom-left or bottom-right or bottom-center or top-left or top-right or top-center or mid-center or an object representing the left, right, top, bottom values
        //position: {
        //    top: 120,
        //    left: 10
        //},
        textAlign: 'left',
        loader: true,
        loaderBg: '#755724',
        beforeShow: function () { },
        afterShown: function () { },
        beforeHide: function () { },
        afterHidden: function () { }
    });
}


function ErrorToast(Text, time) {
    var ToastHidingTime = false;
    if (time !== "") {
        ToastHidingTime = true;
        var TimeSecond = time * 1000;
    }
    $.toast({
        text: Text,
        heading: 'Error',
        icon: 'error',
        showHideTransition: 'slide',
        allowToastClose: true,
        hideAfter: ToastHidingTime === true ? TimeSecond : TostHidingTime,,
        stack: 3,
        position: "top-center", // bottom-left or bottom-right or bottom-center or top-left or top-right or top-center or mid-center or an object representing the left, right, top, bottom values
        //position: {
        //    top: 120,
        //    left: 10
        //},

        textAlign: 'left',
        loader: true,
        loaderBg: '#7e0000',
        beforeShow: function () { },
        afterShown: function () { },
        beforeHide: function () { },
        afterHidden: function () { }
    });
}


function InformationToast(Text, time) {
    var ToastHidingTime = false;
    if (time !== "") {
        ToastHidingTime = true;
        var TimeSecond = time * 1000;
    }
    $.toast({
        text: Text,
        heading: 'Information',
        icon: 'info',
        showHideTransition: 'slide',
        allowToastClose: true,
        hideAfter: ToastHidingTime === true ? TimeSecond : TostHidingTime,,
        stack: 3,
        position: "top-center", // bottom-left or bottom-right or bottom-center or top-left or top-right or top-center or mid-center or an object representing the left, right, top, bottom values
        //position: {
        //    top: 120,
        //    left: 10
        //},
        textAlign: 'left',
        loader: true,
        loaderBg: '#09415d',
        beforeShow: function () { },
        afterShown: function () { },
        beforeHide: function () { },
        afterHidden: function () { }
    });
}



