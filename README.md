#### General request-response flow

<table>
  <tr>
    <td colspan="2">Client side</td>
    <td>Transport</td>
    <td colspan="2">Web service side</td>
  </tr>  
  <tr>
    <td>Client</td>
    <td>Encoder</td>
    <td></td>
    <td>Encoder</td>
    <td>Web service</td>
  </tr>
  <tr>
  <tr>
    <td>1. client code</td>
    <td></td>
    <td></td>
    <td></td>
    <td></td>
  </tr>
    <td>request has <br> been sent </td>
    <td>&#129138; serialization &#129138; 2. logging &#129138;</td>
    <td>&#129138; bytes &#129138;</td>
    <td>&#129138; 3. logging &#129138; deserialization &#129138;</td>
    <td>request <br> received</td>
  </tr>
  <tr>
    <td></td>
    <td></td>
    <td></td>
    <td></td>
    <td>4.service code</td>
  </tr>
  <tr>
    <td>response <br> received</td>
    <td>&#129136; deserialization &#129136; 6. logging &#129136;</td>
    <td>&#129136; bytes &#129136;</td>
    <td>&#129136; 5. logging &#129136 serialization &#129136;</td>
    <td>response <br> returned</td>
  </tr>
  <tr>
    <td>7. client code</td>
    <td></td>
    <td></td>
    <td></td>
    <td></td>
  </tr>
</table>