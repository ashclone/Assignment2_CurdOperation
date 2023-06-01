var dataFromTable;


$(document).ready(function () {  
   
    LoadDataTable();   
    dataFromTable.on("select deselect", function (e, dt, type, indexes) {
        //debugger;
        var rows = dataFromTable.rows({ selected: true }).count();
        if (rows == 0) {
            $("#selectedInfo").hide();
            $("#download").hide();
        }
        else {
            var text = "<h4>" + rows + " rows Selected</h4>"
           
            $("#selectedInfo").html(text);
            $("#selectedInfo").show();
            $("#download").show();
        }
        if ($('#tblData tbody').children('tr').length == $('#tblData tbody').children('tr.selected').length) {
            $('#chkAll').prop('checked', true);
        }
        else {
            $('#chkAll').prop('checked', false);
        }
    });   
    $("#chkAll").change(function () {
        //debugger;
        if ($(this).prop('checked')) {
            dataFromTable.rows().select();
        } else {
            dataFromTable.rows().deselect();
        }       
    })

})
function LoadDataTable() {
    var i = 1;
    $("#selectedInfo").hide();
    dataFromTable = $('#tblData').DataTable({
        dom: 'Bifrtip',   
        order: [[1, 'asc']],
        buttons: [
            {
                extend: 'excelHtml5',
                exportOptions: {
                    rows: { selected: true },
                    columns: [1, 2, 3, 4, 5, 6, 7],
                },               
                title:null,
                init: function (dt) {
                    var that = this;
                    dt.on('select deselect', function () {
                        that.enable(dt.rows({ selected: true }).any());
                    });

                    this.disable();
                },
                customize: function (xlsx) {
                    var sheet = xlsx.xl.worksheets['sheet1.xml'];                   
                    $('row c', sheet).each(function () { $(this).attr('s', '65').attr('s', '51'); });
                    //$('row:A1 c', '').
                    /*$('row:first c', sheet).remove();*/
                    //$('row:A2 c','').attr('s', '2');
                    //$('col:F2 c').attr('s', '51');
                    
                    
                }
            },
            //{
            //    extend: 'pdfHtml5',
            //    exportOptions: {
            //        rows: { selected: true },
            //        columns: [1, 2, 3, 4, 5, 6, 7]
            //    },
            //    init: function (dt, nButton, oConfig) {
            //        var that = this;
            //        dt.on('select deselect  ', function () {
            //            that.enable(dt.rows({ selected: true }).any());
            //        });
            //        this.disable();
            //    }
            //},
            //{
            //    text: 'My button',
            //    action: function (e, dt, node, config) {                    
            //        $.fn.dataTable.ext.buttons.copyHtml5.action.call(this, e, dt, node, config);
            //    },
            //    init: function (dt) {
            //        var that = this;
            //        dt.on('select deselect', function () {
            //            that.enable(dt.rows({ selected: true }).any());
            //        });

            //        this.disable();
            //    }
            //}
        ],       
        "lengthMenu": [20, 30, 40, 50],
       
        "ajax": {
            "url": "/StudentVm/GetAll"
        },        
        "columns": [
            //{
            //    "render": function () {
            //        return i++;
            //    }, "width": "5%"
            //},
            {
                "data": null,
                defaultContent: '',
                className: 'select-checkbox tblchk',
                
            },
                
            { "data": "name", "width": "10%" },
            { "data": "email", "width": "10%" },
            { "data": "address", "width": "10%" },
            { "data": "age", "width": "8%" },
            { "data": "hob", "width": "8%" },
            {
                "data": "salary", "width": "10%",

                render: $.fn.dataTable.render.number(',', '.', 0, '$')

            },
            { "data": "bio", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                            <a href="/studentVm/Upsert/${data}" class="btn btn-success m-2">
                            <i class="fas fa-edit"></i> </a>                           
                            <a class="btn btn-danger" onclick=Delete("/studentVm/Delete/${data}")>
                            <i class="fas fa-trash-alt"></i>
                            </a>
                        </div>

                    `;

                }

            }

        ],        
        select: {
            style: 'multi',            
            info:true,
        },

        'columnDefs': [{
            'targets': [0,8],
            'orderable': false, // orderable false
        }]

    });
    //dataFromTable.button('.buttons-excel').enable(
    //    dataFromTable.rows({ selected: true }).indexes().length === 0 ? false : true
    //);

    //dataFromTable.on('select', function (e, dt, type, indexes) {
    //     dataFromTable.rows({ selected: true }).indexes().length === 0 ? false : true
    //    if (type === 'row') {
    //        dataFromTable.button('.buttons-excel').enable(
    //            dataFromTable.rows({ selected: true }).indexes().length === 0 ? false : true);
    //        // do something with the ID of the selected items
    //    }
    //});

    

    $("#download").click(function () {
      // debugger;
        
        var selectedRows = dataFromTable.rows({ selected: true }).data().toArray();
        console.log(selectedRows);
        var datastringf = JSON.stringify(selectedRows);
        //var queryString = $.param(selectedRows);
       // var url = '/StudentVm/DownloadExcel?' + queryString;
       
        $.ajax({
            type: 'POST',
            url: '/StudentVm/DownloadExcel',
            data: datastringf,
            /*dataType: "json",*/
            contentType: "application/json; charset=utf-8",         
            success: function (response) {
                debugger;
                try {
                    function base64ToArrayBuffer(base64) {
                        var binaryString = window.atob(base64);
                        var binaryLen = binaryString.length;
                        var bytes = new Uint8Array(binaryLen);
                        for (var i = 0; i < binaryLen; i++) {
                            var ascii = binaryString.charCodeAt(i);
                            bytes[i] = ascii;

                        }
                        return bytes;
                    }
                    //var json = JSON.parse(response.data.fileContents);
                    //var data = json.data;
                    // Handle the data
                    var bytes = base64ToArrayBuffer(response.data.fileContents);
                    //var bytes = new Uint8Array(response.data);
                    var blob = new Blob([bytes], { type: response.data.contentType });
                    var link = document.createElement('a'); 
                    const fileobj = URL.createObjectURL(blob);
                    link.href = fileobj;
                    link.download = response.data.fileDownloadName;
                    link.click();

                   
                } catch (e) {
                    // Handle the parse error
                    console.warn(e);
                }
                
               
            },
            error: function (xhr, status, error) {
                // Handle error response
                console.log("status "+status);
                console.log("Error" + error);
                console.log("xhr  " + xhr);
                Swal.fire({
                    title: 'Cancelled',
                    text: "Error in downloading",
                    icon: 'error'
                })
            }
        });
        //$.ajax({
        //    url: "/StudentVm/DownloadExcel",
        //    type: 'POST',
        //    data: rows,
        //    success: function (data) {
        //        if (data.success) {
        //            Swal.fire({
        //                title: 'Downloaded !',
        //                text: 'Excel downloaded',
        //                icon: 'success'
        //            }
        //            )
        //        }
        //        else{
        //            Swal.fire({
        //                title: 'Cancelled',
        //                text: "Error in downloading",
        //                icon: 'error'
        //            }
        //            )
        //        }
        //    }

        //})

        // window.open("https://localhost:7223/api/Vm/DownloadExcel");

    });
}



function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    if (data.success) {
                        Swal.fire({
                            title: 'Deleted!',
                            text: 'data Deleted Successfully ',
                            icon: 'success'
                        }
                        )
                        dataFromTable.ajax.reload();
                    }
                    else {
                        Swal.fire({
                            title: 'Cancelled',
                            text: "Error while Deleting data in DataBase",
                            icon: 'error'
                        }
                        )
                    }
                }
            })

        }
    })
}
