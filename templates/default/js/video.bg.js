(function($){$.fn.videoBG=function(selector,options){if($.fn.isMobileSafari())
return;if(!$.fn.canPositionFixed())
return
var options={};if(typeof selector=="object"){options=$.extend({},$.fn.videoBG.defaults,selector);}
else if(!selector){options=$.fn.videoBG.defaults;}
else{return $(selector).videoBG(options);}
var container=$(this);if(!container.length)
return;if(container.css('position')=='static'||!container.css('position'))
container.css('position','relative');if(options.width==0)
options.width=container.width();if(options.height==0)
options.height=container.height();var wrap=$.fn.videoBG.wrapper();wrap.height(options.height).width(options.width);if(options.textReplacement){options.scale=true;container.width(options.width).height(options.height).css('text-indent','-9999px');}
else{wrap.css('z-index',options.zIndex+1);}
wrap.html(container.html());var video=$.fn.videoBG.video(options);if(options.scale){wrap.height(options.height).width(options.width);video.height(options.height).width(options.width);}
container.html(wrap);container.append(video);return this;}
$.fn.videoBG.video=function(options){var $div=$('<div/>');$div.addClass('videoBG').css('position',options.position).css('z-index',options.zIndex).css('top',0).css('left',0).css('height',options.height).css('width',options.width).css('opacity',options.opacity).css('overflow','hidden');var $video=$('<video/>');$video.css('position','absolute').css('z-index',options.zIndex).attr('poster',options.poster).css('top',0).css('left',0).css('min-width','100%').css('min-height','100%');if(options.autoplay){$video.attr('autoplay',options.autoplay);}
var v=$video[0];if(options.loop=='loop')
{$video.attr('loop','loop');$video.bind('ended',function(){$(this)[0].play();});}
else
{if(options.loop)
{loops_left=options.loop;$video.bind('ended',function(){if(loops_left)
$(this)[0].play();if(loops_left!==true)
loops_left--;});}}
$video.bind('canplay',function(){if(options.autoplay)
v.play();});if($.fn.videoBG.supportsVideo()){if($.fn.videoBG.supportType('webm')){$video.attr('src',options.webm);}
else if($.fn.videoBG.supportType('mp4')){$video.attr('src',options.mp4);}
else{$video.attr('src',options.ogv);}}
var $img=$('<img/>');$img.attr('src',options.poster).css('position','absolute').css('z-index',options.zIndex).css('top',0).css('left',0).css('min-width','100%').css('min-height','100%');if($.fn.videoBG.supportsVideo()){$div.html($video);}
else{$div.html($img);}
if(options.scale){$div.css('height','100%').css('width','100%');$video.css('height','100%').css('width','100%');$img.css('height','100%').css('width','100%');}
if(options.textReplacement){$div.css('min-height',1).css('min-width',1);$video.css('min-height',1).css('min-width',1);$img.css('min-height',1).css('min-width',1);$div.height(options.height).width(options.width);$video.height(options.height).width(options.width);$img.height(options.height).width(options.width);}
if($.fn.videoBG.supportsVideo()){v.play();}
return $div;}
$.fn.videoBG.supportsVideo=function(){return(document.createElement('video').canPlayType);}
$.fn.videoBG.supportType=function(str){if(!$.fn.videoBG.supportsVideo())
return false;var v=document.createElement('video');switch(str){case'webm':return(v.canPlayType('video/webm; codecs="vp8, vorbis"'));break;case'mp4':return(v.canPlayType('video/mp4; codecs="avc1.42E01E, mp4a.40.2"'));break;case'ogv':return(v.canPlayType('video/ogg; codecs="theora, vorbis"'));break;}
return false;}
$.fn.videoBG.wrapper=function(){var $wrap=$('<div/>');$wrap.addClass('videoBG_wrapper').css('position','absolute').css('top',0).css('left',0);return $wrap;}
$.fn.videoBG.defaults={mp4:'',ogv:'',webm:'',poster:'',autoplay:true,loop:5,scale:false,position:"absolute",opacity:1,textReplacement:false,zIndex:0,width:0,height:0}})(jQuery);$.fn.isMobileSafari=function(){return(navigator.userAgent.match(/(iPod|iPhone|iPad)/));};$.fn.canPositionFixed=function(){var container=document.body;if(document.createElement&&container&&container.appendChild&&container.removeChild){var el=document.createElement('div');if(!el.getBoundingClientRect)return null;el.innerHTML='x';el.style.cssText='position:fixed;top:100px;';container.appendChild(el);var originalHeight=container.style.height,originalScrollTop=container.scrollTop;container.style.height='3000px';container.scrollTop=500;var elementTop=el.getBoundingClientRect().top;container.style.height=originalHeight;var isSupported=(elementTop===100);container.removeChild(el);container.scrollTop=originalScrollTop;return isSupported;}
return null;}