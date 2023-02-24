//--------Configuring quill text editor for writing posts------------

var toolbar_options = [
    [{ 'font': [] }],
    [{ 'size': [false, 'large', 'huge'] }],
    [{ 'header': [1, 2, 3, 4, false] }],
    ['bold', 'italic', 'underline', 'strike'],
    [{ 'color': [] }, { 'background': [] }],
    ['blockquote', 'code-block', 'link', 'image'],
    [{ 'list': 'ordered' }, { 'list': 'bullet' }],
    [{ 'align': [] }, 'clean'],
];

var quill_options = {
    modules: {
        toolbar: toolbar_options
    },
    theme: 'snow'
}

//--------Creating quill object and edit it's appearance--------
var quill = new Quill('#post-writer', quill_options);

$('.ql-toolbar').addClass('border rounded-1 border-danger');

//--------Set Max-Min characters limit--------
const max_characters = 5000;
const min_characters = 20;

function displayCharactersInfo() {
    $('#characters-counter').text('Characters: ' + quill.getLength() + ' of ' + max_characters);
    $('#charlenght_valid').text(''); // clear error warning
}

$(document).ready(displayCharactersInfo());

quill.on('text-change', function (delta, old, source) {
    if (quill.getLength() > max_characters) {
        quill.deleteText(max_characters, quill.getLength());
    }

    displayCharactersInfo(); // update info
});

//--------Set value to the hidden input element to send #post-writer html structure--------
$('form').submit(function (event) {
    if (quill.getLength() < min_characters) {
        event.preventDefault();
        $('#charlenght_valid').text('Post should be at least ' + min_characters+ ' characters in length!');
    }
    else $('#post-writer-html').val($('#post-writer').html());
});