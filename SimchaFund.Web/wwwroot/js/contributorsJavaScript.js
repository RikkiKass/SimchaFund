$(() => {

    $("#new-contributor").on('click', function () {
        $("#con-modal").modal();
    });

    $(".deposit-button").on('click',  function () {
        const button = $(this);
        const contributorName = button.data('contributor-name');
        console.log(contributorName)
        const contributorId = button.data('contributor-id');
        console.log(contributorId)
        $("#deposit-name").append(contributorName);
        
        $("#dep-modal").modal();
        $("#con-id").attr('value', contributorId);
        $("commit").on('click', function () {
            $(".modal").modal('hide');
        })

    });
    $("table").on('click', "#edit-con", function () {
        
        const button = $(this);
        const firstName = button.data('first-name');
       
        const cellNumber = button.data('cell');
        const alwaysInclude = button.data('always-include');

        $("#contributor-always-include").prop('checked', alwaysInclude === "True"); 
    
     
        const date = button.data('date');
        const id = button.data('id');
        console.log(id);
        
      
        $("#contributor-first-name").val( `${firstName}`);
        $("#contributor-cell-number").val(`${cellNumber}`);
        $("#contributor-created-at").val(date);
        $("#contributor-id").attr('value', id);
       
        

        $(".modal").modal();
    });

});