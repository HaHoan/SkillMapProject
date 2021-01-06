const SUCCESS = 1;
const ERROR = 2;
$(function () {
    GetAllSubjectOfStaff();
    getDate();
})
function GetAllSubjectOfStaff() {
    var userID = $('#userID').val();
    $.ajax({
        url: "/Home/GetAllSubjectOfStaff",
        data: { ID: userID },
        success: function (response) {
            if (response.code == SUCCESS)
                $('#tabs-2').html(response.message);
        },
        error: function (e) {
            console.log(e);
        }
    });
}

function TabCerClick() {
    $("#li_cer").addClass("active");
    $("#li_skill").removeClass("active");
}
function TabSkillClick() {
    $("#li_skill").addClass("active");
    $("#li_cer").removeClass("active");
}
function addSkillMapForStaff(Ma, Ten, NgayThamGia) {
    $('#MaMonHoc').val(Ma);
    $('#TenMonHoc').val(Ten);
    $('#NgayThamGia').val(NgayThamGia);
    $('#modalAddSkillMapForStaff').modal();
}
function ThemSkillMap() {
    var MaMonHoc = $('#MaMonHoc').val();
    var NgayThamGia = $('#NgayThamGia').val();
    var userID = $('#userID').val();
    var StaffCode = $('#StaffCode').text();
    var GhiChu = $('#GhiChu').val();
    $.ajax({
        url: "/Home/AddSkillMap",
        type: 'POST',
        data: {
            MaMonHoc: MaMonHoc,
            NgayThamGia: NgayThamGia,
            userID: userID,
            StaffCode: StaffCode,
            GhiChu: GhiChu
        },
        success: function (response) {
            if (response.code == SUCCESS)
                $('#tabs-2').html(response.message);
        },
        error: function (e) {
            console.log(e);
        }
    });
    $('#modalAddSkillMapForStaff').modal('toggle');
}