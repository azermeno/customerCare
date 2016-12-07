<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ciudadano.aspx.cs" Inherits="CustomerCare.ciudadano" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="es" lang="es" dir="ltr">



<head> 
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

  <title>Denuncias a Servidores Públicos | Guadalajara - Gobierno Municipal</title>

  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" /> 
 
    <link href="Styles/ciudadano.css" rel="stylesheet" type="text/css" />
</head>

<body class="not-front not-logged-in node-type-webform page-denuncias-servidores-p-blicos section-denuncias-servidores-p-blicos one-sidebar sidebar-second">




          
<div class="webform-confirmation"> 
      <p>La Oficina de Combate a la Corrupción ha recibido exitosamente tu denuncia.</p> 
<br /> 
<p>Para cualquier duda o aclaración puedes comunicarte al teléfono 38-18-36-00, extensión 3672 o al correo electrónico: anticorrupcion@guadalajara.gob.mx 
<br /> 
<h2>Folio: <%= Request.QueryString["fl"] %></h2>  </div> 
 
<div class="links"> 
  <a href="/denuncias-servidores-p-blicos">Volver atrás al formulario</a> 
</div> 
        

   
</body>
</html>
