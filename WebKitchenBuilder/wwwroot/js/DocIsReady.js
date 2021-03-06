﻿$(document).ready(function () {
    //From script3
    document.getElementById("nova").addEventListener("click", dodajGrupu);

    // From DataConnector
    $('#clientID').click(instantLogin);

    // create activity on click - just for testing purpose. It will be deleted after bug solving
    $('#createBundle').click(createAppBundleActivity);
    $('#createBundle').css("display", "none");

    // From ForgeTree
    //prepareAppBucketTree(); za sada isključujem ali je pozivam nakon logovanja
    $('#refreshBuckets').click(function () {
        $('#appBuckets').jstree(true).refresh();
    });

    $('#createNewBucket').click(function () {
        createNewBucket();
    });

    $('#hiddenUploadField').change(function () {
        var node = $('#appBuckets').jstree(true).get_selected(true)[0];
        var _this = this;
        if (_this.files.length == 0) return;
        var file = _this.files[0];
        switch (node.type) {
            case 'bucket':
                var formData = new FormData();
                formData.append('fileToUpload', file);
                formData.append('bucketKey', node.id);

                $.ajax({
                    url: '/api/forge/oss/objects',
                    data: formData,
                    processData: false,
                    contentType: false,
                    type: 'POST',
                    success: function (data) {
                        $('#appBuckets').jstree(true).refresh_node(node);
                        _this.value = '';
                    }
                });
                break;
        }
    });

    // From ForgeDesignAutomation
    $('#startWorkitem').click(startWorkitem);
    $('#forgeViewer').click(animateSelectedElement);
    startConnection();
});