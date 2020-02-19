$(document).ready(function(){
    $(document).on("click", 'a[href^="#"]', function(event){
		event.preventDefault();
		var target = this.hash;
		$('html,body').animate({scrollTop:$(target).offset().top - 80}, 500)
	});
});