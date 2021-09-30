$(document).ready(function() {
   tab();
});
function tab() {
    $('.tab_content').hide();
    $('.tab_content:first').show();
    $('.tab_nav li a:first').addClass('active');
	$('.tab_nav li div:first').addClass('arrow');
    $('.tab_nav li a').click(function(){
       var  id_content = $(this).attr("href"); 
	   var  id_arrow = $(this).attr("title"); 
       $('.tab_content').hide();
       $(id_content).fadeIn();
       $('.tab_nav li a').removeClass('active');
       $(this).addClass('active');
	   $('.arrowDiv').hide();
	   $(id_arrow).show();
	   $('div'+id_arrow).removeClass('arrow');
	   $('div'+id_arrow).addClass('arrow');
       return false;
    });
}// JavaScript Document