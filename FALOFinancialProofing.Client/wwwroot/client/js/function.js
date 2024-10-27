$(document).ready(function () {
    $('#searchTitle').on('input', function () {
        var searchTerm = $(this).val().toLowerCase();
        $('.book-item').each(function () {
            var title = $(this).find('.book-title').text().toLowerCase();
            $(this).toggle(title.includes(searchTerm));
        });
    });

    $('#searchAuthor').on('input', function () {
        var searchTerm = $(this).val().toLowerCase();
        $('.book-item').each(function () {
            var author = $(this).find('.book-author').text().toLowerCase();
            $(this).toggle(author.includes(searchTerm));
        });
    });

    $('#sortByName').change(function () {
        var order = $(this).val();
        var items = $('.book-item').get();

        items.sort(function (a, b) {
            var nameA = $(a).find('.book-title').text().toLowerCase();
            var nameB = $(b).find('.book-title').text().toLowerCase();
            if (order === 'asc') {
                return nameA < nameB ? -1 : nameA > nameB ? 1 : 0;
            } else {
                return nameA > nameB ? -1 : nameA < nameB ? 1 : 0;
            }
        });

        $.each(items, function (index, item) {
            $(item).parent().append(item);
        });
    });
});