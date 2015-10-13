$(document).ready(function () {
    $('a[hreflang=delete_href]').click(function () {
        var answer = confirm('Confirm Delete ?');
        if (answer) {
            var id = $(this).attr("id");
            var url = $(this).attr("data-url");
            $.ajax({
                type: "POST",
                url: "../../" + url + "/Delete",
                data: "ID=" + id,
                success: function (result) {
                    if (result.status == true) {
                        $('#row' + id).remove();
                        ShowMessage(result.message, true);
                    }
                    else {
                        ShowError(result.message, true);
                    }
                },
                error: function (xhr, status, error) {
                    ShowError(xhr.statusText, true);
                }
            });
        }
        return false;
    });
    //$.datepicker.setDefaults({ dateFormat: 'yy/mm/dd' });
    //$(".datepicker").datepicker();

    //bootstrap-datetimepicker
    $('.datetimepicker5').datetimepicker({
        //fomat: "MM/dd/YYYY"
    });
})