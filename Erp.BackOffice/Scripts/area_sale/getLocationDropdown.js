
// cập nhật quận theo tp, và phường theo quận
$(function () {
    var url = '/api/BackOfficeServiceAPI/FetchLocation';

    city.change(function () {
        var id = $(this).val(); // Use $(this) so you don't traverse the DOM again
        $.getJSON(url, { parentId: id }, function (response) {
            districts.empty(); // remove any existing options
            ward.empty();
            $(document.createElement('option'))
                    .attr('value', '')
                    .text('- Rỗng -')
                    .appendTo(ward);
            $(response).each(function () {
                $(document.createElement('option'))
                    .attr('value', this.Id)
                    .text(capitalizeFirstAllWords(this.Name.toLowerCase().replace('huyện', '').replace('quận', '')))
                    .appendTo(districts);
            });
            var $option = $('#ContactId').find('option:selected');
            districts.val($option.data('district'));
            districts.trigger("chosen:updated");
            districts.trigger('change');
        });
    });

    districts.change(function () {
        var id = $(this).val(); // Use $(this) so you don't traverse the DOM again
        $.getJSON(url, { parentId: id }, function (response) {
            ward.empty(); // remove any existing options
            $(response).each(function () {
                $(document.createElement('option'))
                    .attr('value', this.Id)
                    .text(capitalizeFirstAllWords(this.Name.toLowerCase()))
                    .appendTo(ward);
            });
            var $option = $('#ContactId').find('option:selected');
            ward.val($option.data('ward'));
            ward.trigger("chosen:updated");
        });
    });
});