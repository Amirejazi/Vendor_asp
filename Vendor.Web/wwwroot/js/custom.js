function open_waiting(selector = 'body', effect = 'facebook', text = '', bg_color = 'rgba(255,255,255,0.7)', color = '#000') {
    $(selector).waitMe({
        effect: effect,
        text: text,
        bg: bg_color,
        color: color
    });
}

function close_waiting(selector = 'body') {
    $(selector).waitMe('hide');
}

function FillPageId(pageId) {
    $("#PageId").val(pageId);
    $("#form-filter").submit();
}


$(document).ready(function () {
    var editors = $("[ckeditor]");
    if (editors.length > 0) {
        $.getScript('/js/ckeditor.js', function () {
            $(editors).each(function (index, value) {
                var id = $(value).attr('ckeditor');
                ClassicEditor.create(document.querySelector('[ckeditor="' + id + '"]'),
                    {
                        toolbar: {
                            items: [
                                'heading',
                                '|',
                                'bold',
                                'italic',
                                'link',
                                '|',
                                'fontSize',
                                'fontColor',
                                '|',
                                'imageUpload',
                                'blockQuote',
                                'insertTable',
                                'undo',
                                'redo',
                                'codeBlock'
                            ]
                        },
                        language: 'fa',
                        table: {
                            contentToolbar: [
                                'tableColumn',
                                'tableRow',
                                'mergeTableCells'
                            ]
                        },
                        licenseKey: '',
                        simpleUpload: {
                            // The URL that the images are uploaded to.
                            uploadUrl: '/ckupload-image/'
                        }

                    })
                    .then(editor => {
                        window.editor = editor;
                    }).catch(err => {
                        console.error(err);
                    });
            });
        });
    }

    $('#Orderby').on('change', function () {
        $("#form-filter").submit();
    });
});

function changeProductBaseOnColor(colorId, priceOfColor, colorName) {
    var basePrice = parseInt($('#ProductBasePrice').val());
    $('.current_price').html((basePrice + priceOfColor) + 'تومان' + ' (' + colorName + ')');
    $('#add_product_to_order_ProductColorId').val(colorId);
}

$('#number_of_products_in_basket').on('change', function (e) {
    var numberOfProducts = parseInt(e.target.value, 0);
    $('#add_product_to_order_Count').val(numberOfProducts);
});

function OnSuccessAddProductToOrder(res) {
    if (res.status == "Success") {
        ShowMessage('موفقیت', res.message);
    }
    if (res.status == "Danger") {
        ShowMessage('خطا', res.message, 'warning');
    }
    setTimeout(function () {
        close_waiting('#submitOrderForm');
    }, 3000);

    // close_waiting();
}

$('#submitOrderForm').on('click', function () {
    $('#AddProductToOrder').submit();
    // open_waiting('#submitOrderForm');

    open_waiting(
        selector = '#submitOrderForm',
        effect = 'bounce',
        text = '',
        bg_color = 'rgb(162, 218, 245)',
        color = '#FFFFFF');

});


