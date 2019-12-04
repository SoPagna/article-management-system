// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(function () {
    const $articleTable = $('#article-table');

    $articleTable.on('click', '.delete', e => {
        e.preventDefault();

        if (confirm('Are you sure you want to delete this article?')) {
            window.location = e.currentTarget.href;
        }
    });
});
