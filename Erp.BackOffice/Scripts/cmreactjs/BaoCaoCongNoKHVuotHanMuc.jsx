function ConvertSoAm(input_date) {
    var convertdate = "";
    if (parseInt(input_date) < 0) {
        var aa = parseInt(input_date) * (-1);
        var bb = aa.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
        convertdate = "(" + bb + ")";
    }
    else {
        convertdate = parseInt(input_date).toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
    }
    return convertdate;
}
function GetDatatable(url, title) {
      var StartDate = $('#StartDate').val();  
 var EndDate = $('#EndDate').val();  
  var CustomerId = $('#CustomerId').data("id") == undefined ? "" : $('#CustomerId').data("id");  
   ShowLoading();
        $.getJSON(url, {
        StartDate: StartDate,  
EndDate: EndDate,  
CustomerId: CustomerId

        }).done(function (data) {
            ReactDOM.render(
            <TableReport datatable={data} />,
        document.getElementById('react_report')
          );
		  HideLoading();
            });


      var TableReport = React.createClass({
 render: function() {
 
 sumCustomerId=0;
 sumConLai=0;
 sumDaThu=0;
 sumMoneyLimit=0;
 sumMoneyVuot=0;
 sumCustomerName=0;
 var tbody = this.props.datatable.map(function(comment_obj, i) {
 
 sumCustomerId += comment_obj.CustomerId;
 sumConLai += comment_obj.ConLai;
 sumDaThu += comment_obj.DaThu;
 sumMoneyLimit += comment_obj.MoneyLimit;
 sumMoneyVuot += comment_obj.MoneyVuot;
 sumCustomerName += comment_obj.CustomerName;
   return (
  <BodyTable stt ={i + 1}
 
CustomerId= { comment_obj.CustomerId }
CustomerCode= { comment_obj.CustomerCode }
ConLai= { comment_obj.ConLai }
DaThu= { comment_obj.DaThu }
MoneyLimit= { comment_obj.MoneyLimit }
MoneyVuot= { comment_obj.MoneyVuot }
CustomerName= { comment_obj.CustomerName }
   key ={i}>
    </BodyTable>
   );
    });
  return (
   <table className ="table table-bordered table-responsive" id = "cTable">
 <thead>
  <tr>
  <th colSpan = "8" className ="cell-center">< h4 >{ title} từ { StartDate} đến {EndDate}</h4></th>
  </tr>
  <tr>
 <th> STT </th>

 <th className="text-center">Mã khách hàng</th>
 <th className="text-center">Khách hàng</th>
 <th className="text-center">Tổng nợ</th>
 <th className="text-center">Đã thanh toán</th>
 <th className="text-center">Giới hạn nợ</th>
 <th className="text-center">Số tiền vượt mức</th>
  </tr>
   </thead>
  <tbody>
  {tbody}
  <tr className="tr-bold">
  <td></td>
  
 <td className ="cell-center text-right"></td> 
  <td className ="cell-center text-right"></td> 
   <td className="text-right">{ConvertSoAm(sumConLai)}</td>
   <td className="text-right">{ConvertSoAm(sumDaThu)}</td>
   <td className="text-right">{ConvertSoAm(sumMoneyLimit)}</td>
   <td className="text-right">{ConvertSoAm(sumMoneyVuot)}</td>
   </tr>
    </tbody>
    </table>
     );
     }
     });
     };

     
  var BodyTable = React.createClass({
    render: function() {
     var name = this.props.stt % 2 == 0 ? "alert-warning" : "";
    var color_CustomerId = parseInt(this.props.CustomerId) < 0 ? "text-right red":"text-right green"; 
  var color_ConLai = parseInt(this.props.ConLai) < 0 ? "text-right red":"text-right green"; 
  var color_DaThu = parseInt(this.props.DaThu) < 0 ? "text-right red":"text-right green"; 
  var color_MoneyLimit = parseInt(this.props.MoneyLimit) < 0 ? "text-right red":"text-right green"; 
  var color_MoneyVuot = parseInt(this.props.MoneyVuot) < 0 ? "text-right red":"text-right green"; 
  var color_CustomerName = parseInt(this.props.CustomerName) < 0 ? "text-right red":"text-right green"; 

  return (
   <tr className ={name}>
  <td>{this.props.stt}</td>
   
  <td>{this.props.CustomerCode}</td>
    <td>{this.props.CustomerName}</td>
   <td className ={color_ConLai}>{ConvertSoAm(this.props.ConLai)}</td>
   <td className ={color_DaThu}>{ConvertSoAm(this.props.DaThu)}</td>
   <td className ={color_MoneyLimit}>{ConvertSoAm(this.props.MoneyLimit)}</td>
   <td className ={color_MoneyVuot}>{ConvertSoAm(this.props.MoneyVuot)}</td>
   </ tr >
   );
   }
   });