// ================================================================
//  Description: Avatar Upload supporting script
//  License:     MIT - check License.txt file for details
//  Author:      Codative Corp. (http://www.codative.com/)
// ================================================================
var jcrop_api,
    boundx,
    boundy,
    xsize,
    ysize;

$(function () {
    if (typeof $('#avatar-upload-form') !== undefined) {
        var maxSizeAllowed = 2; // ToDo - Change upload limit in MB

        initAvatarUpload();

        $('#avatar-max-size').html(maxSizeAllowed);

        $("#avatar-upload-form input:file").on("change", function (e) {
            var files = e.currentTarget.files;

            for (var x in files) {
                if (files[x].name != "item" && typeof files[x].name != "undefined") {
                    if (files[x].size <= maxSizeAllowed * 1024 * 1024) {
                        // Submit the selected file
                        $('#avatar-upload-form .upload-file-notice').removeClass('bg-danger');
                        $('#avatar-upload-form').submit();
                    } else {
                        // File too large
                        $('#avatar-upload-form .upload-file-notice').addClass('bg-danger');
                    }
                }
            }
        });
    }
});

function initAvatarUpload() {
    $('#avatar-upload-form').ajaxForm({
        beforeSend: function () {
            updateProgress(0);
            $('#avatar-upload-form').addClass('hidden');
        },
        uploadProgress: function (event, position, total, percentComplete) {
            updateProgress(percentComplete);
        },
        success: function (data) {
            updateProgress(100);
            if (data.success === false) {
                $('#status').html(data.errorMessage);
            } else {
                $('#preview-pane .preview-container img').attr('src', data.fileName);

                var img = $('#crop-avatar-target');
                img.attr('src', data.fileName);

                $('#avatar-upload-box').addClass('hidden'); // ToDo - Remove if you want to keep the upload box
                $('#avatar-crop-box').removeClass('hidden');

                initAvatarCrop(img);
            }
        },
        complete: function (xhr) {
        }
    });
}

function updateProgress(percentComplete) {
    $('.upload-percent-bar').width(percentComplete + '%');
    $('.upload-percent-value').html(percentComplete + '%');
    if (percentComplete === 0) {
        $('#upload-status').empty();
        $('.upload-progress').removeClass('hidden');
    }
}

function initAvatarCrop(img) {
    img.Jcrop({
        onChange: updatePreviewPane,
        onSelect: updatePreviewPane,
        aspectRatio: xsize / ysize
    }, function () {
        var bounds = this.getBounds();
        boundx = bounds[0];
        boundy = bounds[1];

        jcrop_api = this;
        jcrop_api.animateTo([100, 100, 200, 200]);
        jcrop_api.setOptions({ allowSelect: true });
        jcrop_api.setOptions({ allowMove: true });
        jcrop_api.setOptions({ allowResize: true });
        jcrop_api.setOptions({ aspectRatio: 1 });

        var pcnt = $('#preview-pane .preview-container');
        xsize = pcnt.width();
        ysize = pcnt.height();

        $('#preview-pane').appendTo(jcrop_api.ui.holder);

        jcrop_api.focus();
    });
}

function updatePreviewPane(c) {
    if (parseInt(c.w) > 0) {
        var rx = xsize / c.w;
        var ry = ysize / c.h;

        $('#preview-pane .preview-container img').css({
            width: Math.round(rx * boundx) + 'px',
            height: Math.round(ry * boundy) + 'px',
            marginLeft: '-' + Math.round(rx * c.x) + 'px',
            marginTop: '-' + Math.round(ry * c.y) + 'px'
        });
    }
}

function saveAvatar() {
    var img = $('#preview-pane .preview-container img');
    $('#avatar-crop-box button').addClass('disabled');

    $.ajax({
        type: "POST",
        url: "/Avatar/Save",
        traditional: true,
        data: {
            w: img.css('width'),
            h: img.css('height'),
            l: img.css('marginLeft'),
            t: img.css('marginTop'),
            fileName: img.attr('src')
        }
    }).done(function (data) {
        if (data.success === true) {
            $('#avatar-result img').attr('src', data.avatarFileLocation);

            $('#avatar-result').removeClass('hidden');
            $('#avatar-crop-box').addClass('hidden'); // ToDo - Remove if you want to keep the upload box
        } else {
            alert(data.errorMessage)
        }
    }).fail(function (e) {
        alert('Cannot upload avatar at this time');
    });
}