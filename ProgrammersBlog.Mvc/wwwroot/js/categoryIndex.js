$(document).ready(function () {

    /*Data Table starts here*/
    $('#categoriesTable').DataTable({
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                text: 'Ekle',
                attr: {
                    id: "btnAdd",
                },
                className: 'btn btn-success',
                action: function (e, dt, node, config) {
                }
            },
            {
                text: 'Yenile',
                className: 'btn btn-warning',
                action: function (e, dt, node, config) {
                    $.ajax({
                        type: 'GET',
                        url: '/Admin/Category/GetAllCategories/',
                        contentType: "application/json",
                        beforeSend: function () {
                            $('#categoriesTable').hide();
                            $('.spinner-border').show();
                        },
                        success: function (data) {
                            const categoryListDto = jQuery.parseJSON(data);
                            console.log(categoryListDto);

                            // === ile hem veri tipini hem de doğruluğunu kontrol ediyoruz.
                            if (categoryListDto.ResultStatus === 0) {
                                let tableBody = "";
                                $.each(categoryListDto.Categories.$values,
                                    function (index, category) {
                                        tableBody += `
                                                <tr>
                                    <td>${category.Id}</td>
                                    <td>${category.Name}</td>
                                    <td>${category.Description}</td>
                                    <td>${convertFirstLetterToUpperCase(category.IsActive.toString())}</td>
                                    <td>${convertFirstLetterToUpperCase(category.IsDeleted.toString())}</td>
                                    <td>${category.Note}</td>
                                    <td>${convertToShortDate(category.CreatedDate)}</td>
                                    <td>${category.CreatedByName}</td>
                                    <td>${convertToShortDate(category.ModifiedDate)}</td>
                                    <td>${category.ModifiedByName}</td>
                                    <td>
                                    <button class="btn btn-primary btn-sm btn-update" data-id="${category.Id}"><span class="fas fa-edit"></span></button>
                                    <button class="btn btn-danger btn-sm btn-delete" data-id="${category.Id}"><span class="fas fa-minus-circle"></span></button>
                                    </td>
                                            </tr>`;
                                    });

                                //html ile yazılanlarla, kendi tablomuzu yer değiştiriyoruz.
                                $('#categoriesTable > tbody').replaceWith(tableBody);
                                $('.spinner-border').hide();
                                $('#categoriesTable').fadeIn(1400);
                            } else {
                                toastr.error(`${categoryListDto.Message}`, 'İşlem Başarısız!');
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            $('.spinner-border').hide();
                            $('#categoriesTable').fadeIn(1000);
                            toastr.error(`${err.responseText}`, 'Hata!');
                        }
                    });
                }
            }
        ],
        language: {
            "sDecimal": ",",
            "sEmptyTable": "Tabloda herhangi bir veri mevcut değil",
            "sInfo": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            "sInfoEmpty": "Kayıt yok",
            "sInfoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "Sayfada _MENU_ kayıt göster",
            "sLoadingRecords": "Yükleniyor...",
            "sProcessing": "İşleniyor...",
            "sSearch": "Ara:",
            "sZeroRecords": "Eşleşen kayıt bulunamadı",
            "oPaginate": {
                "sFirst": "İlk",
                "sLast": "Son",
                "sNext": "Sonraki",
                "sPrevious": "Önceki"
            },
            "oAria": {
                "sSortAscending": ": artan sütun sıralamasını aktifleştir",
                "sSortDescending": ": azalan sütun sıralamasını aktifleştir"
            },
            "select": {
                "rows": {
                    "_": "%d kayıt seçildi",
                    "0": "",
                    "1": "1 kayıt seçildi"
                }
            }
        }
    });
    /* DataTables ends here */

    //formun içindeki verileri categoryaddDto'ya dönüştürüyorum.
    /* Ajax GET / Getting the _CategoryAddPartial as Modal Form strats from here */
    $(function () {
        const url = '/Admin/Category/Add/';
        const placeHolderDiv = $('#modalPlaceHolder');
        $('#btnAdd').click(function () {
            $.get(url).done(function (data) {
                placeHolderDiv.html(data);
                placeHolderDiv.find(".modal").modal('show');
            });
        });

        /* Ajax GET / Getting the _CategoryAddPartial as Modal Form ends here */

        /* Ajax POST / Posting the FormData as CategoryAddDto Form starts here */

        placeHolderDiv.on('click', '#btnSave', function (event) {
            event.preventDefault() // butonun kendi click eventini engellemek için yaptık, mesela submit butonuna basıldığında sayfa yenilenir ben her ne kadar üstüne bir şey yazsamda, bu sayede kendi click işlemini engellemiş oldum

            const form = $('#form-category-add');
            const actionUrl = form.attr('action');
            const dataToSend = form.serialize();

            //done neyi return edeceğini söylüyor
            $.post(actionUrl, dataToSend).done(function (data) {
                const categoryAddAjaxModel = jQuery.parseJSON(data);
                const newFormBody = $('.modal-body', categoryAddAjaxModel.CategoryAddPartial);  //table da class olarak modal body kullanılmış, onu kullanarak seçiyoruz.
                placeHolderDiv.find('.modal-body').replaceWith(newFormBody);

                //attr seçerken [] kullanılır. name'ini ısvalid verdiğimiz labele ulaşmaya çalışıyoruz.
                //sonra da value değerini okuyup ona true değerini atıyoruz.
                const IsValid = newFormBody.find('[name="IsValid"]').val() === 'True';

                if (IsValid) {
                    placeHolderDiv.find('.modal').modal('hide');

                    // `` ile string formatlıyoruz.
                    const newTableRow = `
                            <tr>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.Id}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.Name}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.Description}</td>
                                <td>${convertFirstLetterToUpperCase(categoryAddAjaxModel.CategoryDto.Category.IsActive.toString())}</td>
                                <td>${convertFirstLetterToUpperCase(categoryAddAjaxModel.CategoryDto.Category.IsDeleted.toString())}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.Note}</td>
                                <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.CreatedDate)}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.CreatedByName}</td>
                                <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.ModifiedDate)}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.ModifiedByName}</td>
                                <td>
                                <button class="btn btn-primary btn-sm btn-update" data-id="${categoryAddAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-edit"></span></button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${categoryAddAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-minus-circle"></span></button>
                                </td>
                            </tr>
                      Action          `;

                    //$ ile jquery ya da javascript objesi haline getirmiş oluyoruz.
                    const newTableRowObject = $(newTableRow);
                    newTableRowObject.hide();

                    //# id ile seçmede kullanılıyor
                    $('#categoriesTable').append(newTableRowObject);

                    //efektli gelmesini sağladık.
                    newTableRowObject.fadeIn(3500);
                    toastr.success(`${categoryAddAjaxModel.CategoryDto.Message}`, 'Başarılı İşlem!');
                }
                else {
                    let summaryText = "";
                    $('#validation-summer > ul > li').each(function () {
                        let text = $(this).text();
                        summaryText = `*${text}\n`;
                    });
                    toastr.warning(summaryText);
                }
            });
        });

    });


    /* Ajax POST / Posting the FormData as CategoryAddDto ends here */
    /* Ajax POST / Deleting a Category starts from here */

    //ilk olarak sil butonuna tıklanmayı yakalamalıyız
    $(document).on('click',
        '.btn-delete',
        function (event) {
            event.preventDefault(); //butonun kendi işlevselliğini deactive ettik.
            const id = $(this).attr('data-id'); //click yakalandığında hangi butona basılmış onu alıyoruz this ile sonra da butona verdiğimiz data-id'yi alıyoruz.
            const tableRow = $(`[name="${id}"]`);
            const categoryName = tableRow.find('td:eq(1)').text();  //2.sıradaki table datayı seç dedik (yani name alanı)
            //sweetalert
            Swal.fire({
                title: 'Silmek istediğinize emin misiniz?',
                text: `${categoryName} adlı kategori silinicektir!`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Evet, silmek istiyorum.',
                cancelButtonText: 'Hayır, silmek istemiyorum.'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        data: { categoryId: id },
                        url: '/Admin/Category/Delete/',
                        success: function (data) {
                            const categoryDto = jQuery.parseJSON(data);
                            if (categoryDto.ResultStatus === 0) {
                                Swal.fire(
                                    'Silindi!',
                                    `${categoryDto.Category.Name} adlı kategori başarıyla silinmiştir.`,
                                    'success'
                                );
                                tableRow.fadeOut(3500);
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Başarısız İşlem!',
                                    text: `${categoryDto.Message}`,
                                });
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            toastr.error(`${err.responseText}`, "Hata!");
                        }
                    });
                }
            });
        });

    /*update*/
    $(function () {
        const url = '/Admin/Category/Update/';
        const placeHolderDiv = $('#modalPlaceHolder');

        $(document).on('click',
            '.btn-update',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id'); //eventin gerçekleştiği butonu this ile almış oluyoruz

                //jQuery get
                $.get(url, { categoryId: id }).done(function (data) {
                    placeHolderDiv.html(data); //benim sana yeni verdiğim html ile ekrana ver
                    placeHolderDiv.find('.modal').modal('show');  //bunu göster

                }).fail(function () {
                    toastr.error("Bir hata oluştu");
                });
            });

    });


});