function setFormAction(containingJqueryElement, formAction) {
    containingJqueryElement.closest('form').attr('action', formAction);
}

function setFormActionFromButton(jqueryButton) {
    var formAction = jqueryButton.attr('formaction');

    setFormAction(jqueryButton, formAction);
}

function initPrettyPhoto() {
    $("a[rel^='prettyPhoto']").prettyPhoto();
}

$(document).on('click', '.deleteButton', function (e) {
    var result = confirm('Data will be deleted completely. Are you sure?');

    if (!result) {
        e.preventDefault();
        e.stopPropagation();
        e.stopImmediatePropagation();
    }

    return result;
});

$(document).on('click', '.cancelButton', function (e) {
    var result = confirm('Calculation will be cancelled. Are you sure?');

    if (!result) {
        e.preventDefault();
        e.stopPropagation();
        e.stopImmediatePropagation();
    }

    return result;
});

$(document).on('click', 'form input[formaction]', function (e) {
    setFormActionFromButton($(this));

    if ($(this).attr('data-fullpostback'))
    {
        e.preventDefault();
        var $form = $(this).closest("form");
        $form.attr("data-ajax", "");
        $form.trigger("submit");
        $form.attr("data-ajax", "true");
    }
});

$(document).ready(function () {
    initPrettyPhoto();
});

$(document).on('click', '#showIterationImages', function () {
    var id = $(this).attr('data-id');
    var iterations = $(this).attr('data-iterations');
    var url = $(this).attr('data-url');
    var images = new Array();
    var descriptions = new Array();

    for (var i = 1; i <= iterations; i++) {
        var imageUrl = url + '?calculationId=' + id + '&iteration=' + i + '';
        images.push(imageUrl);
        descriptions.push('<a href="' + imageUrl + '" target="_blank">Iteration ' + i + '</a>');
    }

    $.prettyPhoto.open(images, null, descriptions);

    return false;
});

function formCompleted() {
    initPrettyPhoto();
}