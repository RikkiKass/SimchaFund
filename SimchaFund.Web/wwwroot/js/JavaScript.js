$(() => {
   
    $("#new-simcha").on('click', function () {
        $(".modal").modal();
        $("commit").on('click', function () {
            $(".modal").modal('hide');
        });
    });

    $("#new-contributor").on('click', function () {
        console.log('hi');
        $(".modal-dialog").modal();
    });
    
});