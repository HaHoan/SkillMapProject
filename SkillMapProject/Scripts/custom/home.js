$(function () {
    GetSkillOfStaffList();
    getDate();
    addLevelForSkill($('#skill_selected').val());
    $('#skill_selected').change(function () {
        addLevelForSkill($('#skill_selected').val());
    });
    $('#capdo_selected').change(function () {
        changeLevelByCapDo();
    });
})
function changeLevelByCapDo() {
    var capdo = $('#capdo_selected').val();
    $('#levels_selected').val(capdo);
}
function addLevelForSkill(levels) {
    $('#levels_selected').empty();
    $('#capdo_selected').empty();

    var arr = levels.split(',');
    arr.forEach(function (item, index) {
        if (item != '') {
            $('#levels_selected').append('<option value = "' + index + '" name = "' + item + '">' + item + '</option>');
            $('#capdo_selected').append('<option value = "' + index + '"> Cấp Độ ' + (index + 1) + '</option>');
        }
       
    });
    changeLevelByCapDo();
}
function GetSkillOfStaffList() {
    $(".loading").show();
    $.ajax({
        url: "/Home/GetSkillOfStaffList",
        success: function (response) {
            $('#list_skill_of_staff').html(response.body);
            $(".loading").hide();
        },
        error: function (e) {
            console.log(e);
            $(".loading").hide();
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

function getDate() {
    var today = new Date();
    $('.date').val(today.getFullYear() + '-' + ('0' + (today.getMonth() + 1)).slice(-2) + '-' + ('0' + today.getDate()).slice(-2));
}

var selectedUser = "";
function addSkillForStaff(userId) {
    selectedUser = userId;
}

function submitSkillForStaff() {
    var CapDo = $('#levels_selected').find('option:selected').attr("name");
    var NgayCap = $('#NgayCap').val();
    var NgayThiXacNhan = $('#NgayThiXacNhan').val();
    var SkillID = $('#skill_selected').find('option:selected').attr("name");
    $.ajax({
        url: "/Home/ThemKiNangChoNV",
        type: "POST",
        data: {
            userID: selectedUser,
            CapDo: CapDo,
            NgayCap: NgayCap,
            NgayThiXacNhan: NgayThiXacNhan,
            SkillID: SkillID
        },
        success: function (response) {
            if (response.body == 'OK') {
                GetSkillOfStaffList();
                $('#modalAddSkillForStaff').modal('hide');
            } else if (response.body == "NG") {
                alert("Có lỗi xảy ra!");
            } else {
                alert(response.body);
            }
        },
        error: function (e) {

        }
    });
}