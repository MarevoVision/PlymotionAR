//
//  PermissionChecker.mm
//
//
//  Created by 二宮　匠 on 2017/04/25.
//
//

#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>
#import <AssetsLibrary/AssetsLibrary.h>
#import <AVFoundation/AVFoundation.h>


extern "C"{
    
    //「写真」へのアクセス状況を確認する
    void _PermissionCheck_Use_Camera(const char* parentObjName, const char* callbackName){
        
        NSString* _parentObjName =[NSString stringWithFormat:@"%s", parentObjName];
        NSString* _callbackName =[NSString stringWithFormat:@"%s", callbackName];
        
        AVAuthorizationStatus status = [AVCaptureDevice authorizationStatusForMediaType:AVMediaTypeVideo];
        switch(status){
            case AVAuthorizationStatusAuthorized://カメラが許可されている
                UnitySendMessage([_parentObjName UTF8String],[_callbackName UTF8String],[@"true" UTF8String]);
                break;
                
                
            case AVAuthorizationStatusDenied: //カメラが許可されていない
                UnitySendMessage([_parentObjName UTF8String],[_callbackName UTF8String],[@"false" UTF8String]);
                break;
                
                
            case AVAuthorizationStatusRestricted: //実際にこの値を取得できない
                UnitySendMessage([_parentObjName UTF8String],[_callbackName UTF8String],[@"false" UTF8String]);
                break;
                
        }
    }
    
    
    
    
    //フォトライブラリへのアクセス状況を確認する
    void _PermissionCheck_Use_PhotoLibrary(const char* parentObjName, const char* callbackName){
        
        NSString* _parentObjName =[NSString stringWithFormat:@"%s", parentObjName];
        NSString* _callbackName =[NSString stringWithFormat:@"%s", callbackName];
        
        /*
         if ( floor(NSFoundationVersionNumber) > NSFoundationVersionNumber_iOS_8_x_Max )
         {
         // iOS9以降の場合
         
         PHAuthorizationStatus status = [PHPhotoLibrary authorizationStatus];
         if(status == PHAuthorizationStatusAuthorized){
         UnitySendMessage([_parentObjName UTF8String],[_callbackName UTF8String],[@"true" UTF8String]);
         
         }else if(status == PHAuthorizationStatusDenied){
         UnitySendMessage([_parentObjName UTF8String],[_callbackName UTF8String],[@"false" UTF8String]);
         
         }else if(status == PHAuthorizationStatusRestricted){
         UnitySendMessage([_parentObjName UTF8String],[_callbackName UTF8String],[@"false" UTF8String]);
         
         }else{
         UnitySendMessage([_parentObjName UTF8String],[_callbackName UTF8String],[@"false" UTF8String]);
         
         }
         
         
         } else {*/
        // iOS8以前の場合
        ALAuthorizationStatus status = [ALAssetsLibrary authorizationStatus];
        switch (status) {
            case ALAuthorizationStatusAuthorized:
                UnitySendMessage([_parentObjName UTF8String],[_callbackName UTF8String],[@"true" UTF8String]);
                break;
            case ALAuthorizationStatusDenied:
                UnitySendMessage([_parentObjName UTF8String],[_callbackName UTF8String],[@"false" UTF8String]);
                break;
            case ALAuthorizationStatusRestricted:
                UnitySendMessage([_parentObjName UTF8String],[_callbackName UTF8String],[@"false" UTF8String]);
                break;
            default:
                break;
        }
        //}
        
    }
    
    
    
    
}
