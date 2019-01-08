var baseUrl = "http://localhost:6677/api/";
var myApp = new Framework7({
    tapHold: false
});
var $$ = Dom7;

var mainView = myApp.addView('.view-main', {
    dynamicNavbar: true
});

$(document).ready(function() {
    $("#imgInp").change(function() {
        readURL(this);
    });

    $("#drop-area").dmUploader({
        headers: {
            "Access-Control-Allow-Origin": "*",
            "clientId": getClientId()
        },
        url: baseUrl + "upload",
        onInit: function() {
            console.log('Callback: Plugin initialized');
        },
        onUploadError: function(a, b, c) {
            myApp.alert("", JSON.stringify(a));
        },
        onUploadComplete: function(a, b, c) {
            myApp.hideProgressbar("#uploadProgress");
            $("#resultImage").attr("src", baseUrl + "face");
        },
        onBeforeUpload: function(id) {
            $("#uploadProgress").show();
        },
        onUploadProgress: function(id, percent) {
            myApp.setProgressbar("#uploadProgress", percent, 10);
        }
    });
});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function(e) {
            $('#mainImage').attr('src', e.target.result);
        };
        reader.readAsDataURL(input.files[0]);
    }
}

function getClientId() {
    if (localStorage.getItem("googoolimagooli") !== null && localStorage.getItem("googoolimagooli") !== undefined) {
        return localStorage.getItem("googoolimagooli");
    }
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
    }
    var id = s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    localStorage.setItem("googoolimagooli", id);
    return id;

}

