window.scrollCaptcha = false;

function scrollFixCaptcha() {
    if (window.scrollCaptcha) {
        jQuery("html, body").scrollTop(window.scrollCaptcha);
    }
}

function iOSversion() {
    if (/iP(hone|od|ad)/.test(navigator.platform)) {
        var v = (navigator.appVersion).match(/OS (\d+)_(\d+)_?(\d+)?/);
        return [parseInt(v[1], 10), parseInt(v[2], 10), parseInt(v[3] || 0, 10)];
    }
}

document.addEventListener("scroll", function () {
    var el = document.getElementsByClassName("g-recaptcha")[0];
    var theTop = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop;
    var elemTop = el.getBoundingClientRect().top;
    var elemBottom = el.getBoundingClientRect().bottom;
    var isVisible = (elemTop >= 0) && (elemBottom <= window.innerHeight);
    if (isVisible) {
        window.scrollCaptcha = theTop;
    }
});

function ShowIosCompatibilityIfApplicable() {
    var iosVersion = iOSversion();

    if (iosVersion !== undefined) {
        if (iosVersion[0] === 10 || iosVersion[0] === 11) {
            addAlertInfo("iosAlertPlaceholder", "We have detected you are running iOS " + iosVersion[0] + "." + iosVersion[1] + "." + iosVersion[2]
                + ". For best results, we recommend at least iOS 12");
        }
    }
}