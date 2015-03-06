
h<-10
d<-290


Ddir<-d-h
Ddir

if((Ddir<=180 && Ddir>=0) || (Ddir>=-180 && Ddir<=0)){
	
	kiihdytys<-Ddir/2
	stop<-Ddir
}
if(Ddir>180){ 
	Ddir<-Ddir-360
	kiihdytys<-Ddir/2
	stop<-Ddir
} 

if(Ddir<(-180)){
	Ddir<-Ddir+360
	kiihdytys<-Ddir/2
	stop<-Ddir
}

kiihdytys
stop

Ddir<-h-d

acc<-Ddir*0.45
dec<-Ddir*0.65
