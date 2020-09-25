$(function () {
    $("form[name='create']").validate({       
        rules: {        
            coursename: "required",
            subject: "required",
        },
        messages: {
            coursename: "Please enter the course name",
            subject: "Please enter the subject",
        },
        submitHandler: function (form) {
            form.submit();
        }
    });
});