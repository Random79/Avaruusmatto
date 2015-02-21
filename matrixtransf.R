deg2Rad<-pi/180
rad2Deg<-180/pi

cos(45*deg2Rad)
cos(0*deg2Rad)
cos(0*deg2Rad)

x<-90*deg2Rad
y<-0
z<-0

Rx<-matrix(c(1,0,0, 0,cos(x),-sin(x), 0,sin(x),cos(x)),nrow=3,byrow=TRUE)
Ry<-matrix(c(cos(y),0,sin(y), 0,1,0, -sin(y),0,cos(y)),nrow=3,byrow=TRUE)
Rz<-matrix(c(cos(z),-sin(z),0, sin(z),cos(z),0, 0,0,1),nrow=3,byrow=TRUE)

velVec<-c(0,0,10)
worldpos<-velVec%*%Rz%*%Ry%*%Rx
worldpos


sqrt(7.071068^2+7.071068^2)