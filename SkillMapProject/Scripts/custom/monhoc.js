const TOANCONGTY = 0;
const TAIBOPHAN = 1;
const SUCCESS = 1;
const ERROR = 2;
function AddClick() {
    $("#TenMonHoc").val("");
    $("#MaMonHoc").val("");
    $('#loaihinh').val(0);
    $("#dept").val($("#dept option:first").val());
    loaiHinhChange('#loaihinh','#dept');
    $("#modalAddMonHoc").modal();
}

$(function () {
    $(".loading").hide();
    $('#loaihinh').change(function () {
        loaiHinhChange('#loaihinh','#dept');
    });
    $('#loaihinh_Edit').change(function () {
        loaiHinhChange('#loaihinh_Edit', '#dept_Edit');
    });
})
function loaiHinhChange(id, dept) {
    var loaihinh = $(id+" option:selected").val();
    if (loaihinh == TOANCONGTY) {
        $(dept).prop("disabled", true);
    } else if (loaihinh == TAIBOPHAN) {
        $(dept).prop("disabled", false);
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
function EditClick(MaBoMon, TenBoMon, LoaiMonHoc, Dept) {
    $("#MaMonHoc_Edit").val(MaBoMon);
    $("#TenMonHoc_Edit").val(TenBoMon);
    $("#loaihinh_Edit option").filter(function () {
        return $(this).text() == LoaiMonHoc;
    }).prop('selected', true);
    
    if (Dept == '') {
        $("#dept_Edit").val($("#dept option:first").val());
    } else {
        $("#dept_Edit option").filter(function () {
            return $(this).text() == Dept;
        }).prop('selected', true);
    }
    loaiHinhChange('#loaihinh_Edit','#dept_Edit');
    $("#modalEditMonHoc").modal();
}
function SuaMonHoc() {
    var MaMonHoc = $("#MaMonHoc_Edit").val();
    var TenMonHoc = $("#TenMonHoc_Edit").val();
    var loaihinh = $("#loaihinh_Edit option:selected").text();
    var dept = $("#dept_Edit option:selected").text();
    $.ajax({
        url: "/MonHoc/SuaMonHoc",
        type: "POST",
        data: { MaMonHoc: MaMonHoc, TenMonHoc: TenMonHoc, loaihinh: loaihinh, dept: dept },
        success: function (response) {
            if (response.code == ERROR) {
                alert(response.message);
            } else if (response.code == SUCCESS) {
                $('#modalEditMonHoc').modal('toggle');
                $('#list_monhoc').html(response.message);
            }
        },
        error: function (e) {
            alert(e);
        }
    });
}
var selected_subject;
function DeleteClick(MaBoMon) {
    selected_subject = MaBoMon;
    $("#alertWarning").modal();
}

function XoaMonHoc() {
    $.ajax({
        url: "/MonHoc/XoaMonHoc",
        type: "POST",
        data: { MaBoMon: selected_subject },
        success: function (response) {
            $('#list_monhoc').html(response.message);
        },
        error: function (e) {
            alert(e);
        }
    });
    $('#alertWarning').modal('toggle');
}