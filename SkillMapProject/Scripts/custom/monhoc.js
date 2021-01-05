const TOANCONGTY = 0;
const TAIBOPHAN = 1;
const SUCCESS = 1;
const ERROR = 2;
function AddClick() {
    $("#TenMonHoc").val("");
    $("#MaMonHoc").val("");
    $('#loaihinh').val(0);
    $("#dept").val($("#dept option:first").val());
    loaiHinhChange();
    $("#modalAddMonHoc").modal();
}

$(function () {
    $(".loading").hide();
    $('#loaihinh').change(function () {
        loaiHinhChange();
    });
})
function loaiHinhChange() {
    var loaihinh = $("#loaihinh option:selected").val();
    if (loaihinh == TOANCONGTY) {
        $("#dept").prop("disabled", true);
    } else if (loaihinh == TAIBOPHAN) {
        $("#dept").prop("disabled", false);
    }
}

function AddMonHoc() {
    var MaMonHoc = $("#MaMonHoc").val();
    var TenMonHoc = $("#TenMonHoc").val();
    var loaihinh = $("#loaihinh option:selected").text();
    var dept = $("#dept option:selected").text();
    $.ajax({
        url: "/MonHoc/AddMonHoc",
        type: "POST",
        data: { MaMonHoc: MaMonHoc, TenMonHoc: TenMonHoc, loaihinh: loaihinh, dept: dept },
        success: function (response) {
            if (response.code == ERROR) {
                alert(response.message);
            } else if (response.code == SUCCESS) {
                $('#modalAddMonHoc').modal('toggle');
                $('#list_monhoc').html(response.message);
            }
        },
        error: function (e) {
            alert(e);
        }
    });
    
}
