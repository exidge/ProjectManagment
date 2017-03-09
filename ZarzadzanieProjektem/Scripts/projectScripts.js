var toggledList = [];
//Nested tables and button behavior
$(document).ready(function () {
    reToggle();
    $(".expandButton").click(function () {
        //alert('#tr' + $(this).attr('id'));
        $(this).toggleClass("btn-success btn-danger")
        $(this).find('span').toggleClass("glyphicon-plus-sign glyphicon-minus-sign");
        //$(this).text(function (i, text) {
        //    return text === "+" ? "-" : "+";
        //})
        $('#tr' + $(this).attr('id')).toggle();
        
        
    });
});
function saveToggles() {
    console.log('save');
    $('.collapse').each(function() {
        if ($(this).is(":visible")) {
            toggledList.push($(this).attr('id'));
            console.log($(this).attr('id'));
        }
        localStorage.setItem("toggles", JSON.stringify(toggledList));
    });
}
function unique(array) {
    return $.grep(array, function (el, index) {
        return index === $.inArray(el, array);
    });
}
function reToggle() {
    if (localStorage['toggles']) {
        console.log(JSON.parse(localStorage.getItem("toggles")));
        var toRetoggle = JSON.parse(localStorage.getItem("toggles"));
        toRetoggle.forEach(function (item) {
            $('#' + item).toggle();
        });
        toggledList = [];
    }
    else {
        console.log("no toggles storage")
    }
    localStorage.removeItem('toggles');
}
//"Zaakceptuj" button behavior
function claimFinal(UserId, ProjectId) {
    //alert(id);
    $.ajax({
        url: '/Projects/FinalClaimProject/' + UserId,
        type: "get", //send it through get method
        data: {
            UserId: UserId,
            ProjectId: ProjectId
        },
        success: function (data) {
            saveToggles();
            location.reload(true);
            alert("sucess");
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {
            alert("err");
            // handle status === "timeout"
            // handle other errors
        }
    });
}
//"Odrzuc" Button behavior
function disclaimFinal(UserId, ProjectId) {
    //alert(id);
    $.ajax({
        url: '/Projects/DisclaimProject/' + UserId,
        type: "get", //send it through get method
        data: {
            UserId: UserId,
            ProjectId: ProjectId
        },
        success: function (data) {
            saveToggles();
            location.reload(true);
            alert("sucess");
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {
            alert("err");
            // handle status === "timeout"
            // handle other errors
        }
    });
}