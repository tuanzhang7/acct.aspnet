$(document).ready(function () {
    $("#add").click(function () {
        var new_counter = GetLastCounter();
        var row = '<tr data-counter=' + new_counter + '>';
        row += '<td><div class="editor-long"><input class="form-control" type="text" name="OrderDetail[' + new_counter + '].Description" /></div></td>';
        row += '<td><div class="editor-numeric-short calc"><input class="form-control" type="text" name="OrderDetail[' + new_counter + '].Qty"  value="0"/></div></td>';
        row += '<td><div class="editor-numeric calc"><input class="form-control" type="text" name="OrderDetail[' + new_counter + '].UnitPrice"  value="0"/></div></td>';
        row += '<td><div class="editor-numeric-short calc"><input class="form-control" type="text" name="OrderDetail[' + new_counter + '].Discount"  value="0"/></div></td>';
        row += '<td class="text-right"><div class="editor-numeric"><span name="OrderDetail[' + new_counter + '].Amount" data-id="@(item.Id)"></span></div></td>';
        row += '<td></td>';
        row += '</tr>';

        $('#detail-table  tfoot tr:last').after(row);

    return false;
    });
    $('.calc').find('input').on('blur', function () {
        var id = $(this).data("id");
        calcTotal(id);
    });
    showTotal();
})
function GetLastCounter() {
    var counter = $('#detail-table tfoot tr:last').data("counter");
    return counter + 1;
}
function showTotal() {
    //run through each row
    $('#detail-table  tr').each(function (i, row) {
        var id = $(row).data("id");
        calcTotal(id);
    });
}
function calcTotal(id) {
    var qty = $('[name="OrderDetails[' + id + '].Qty"]').val();
    var unitprice = $('[name="OrderDetails[' + id + '].UnitPrice"]').val();
    var discount = $('[name="OrderDetails[' + id + '].Discount"]').val();

    var total = getTotal(qty, unitprice, discount);
    $('[name="OrderDetails[' + id + '].Amount"]').html(total);
}
function getTotal(qty, unitprice, discount) {
    var total = qty * unitprice;
    if (discount > 0 && discount < 100) {
        total = total * (100 - discount) / 100;
    }
    return total.toFixed(2);
}