$(() => {
    function EnsureFormValidity() {
        let name = $("#name").val();
        let comment = $("#comment").val();
        const isValid = name && comment;
        $("#submit").prop('disabled', !isValid)
    }

    $("#name").on('input', function () {
        EnsureFormValidity();
    });

    $("#comment").on('input', function () {
        EnsureFormValidity();
    });
});