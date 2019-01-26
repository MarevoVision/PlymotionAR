//
//  SavingPhotoEvent.mm
//

#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>
#import <AssetsLibrary/AssetsLibrary.h>
#import <AVFoundation/AVFoundation.h>
#import <AVFoundation/AVAudioSession.h>

extern "C"{
    
    AVAudioPlayer *audioPlayer; //シャッター音で使う
    
    //シャッター音再生
    void _PlaySystemShutterSound(){
        
        AVAudioSession *audioSession = [AVAudioSession sharedInstance];
        [audioSession setCategory:AVAudioSessionCategoryPlayback error:nil];
        [audioSession setActive:YES error:nil];
        
        if (@available(iOS 10.0, *)){
            NSLog(@"iOS10.0+");
            
            NSString *path = @"/System/Library/Audio/UISounds/photoShutter.caf";
            NSURL *playFileURL = [NSURL fileURLWithPath:path];
            NSError *error = nil;
            audioPlayer = [[AVAudioPlayer alloc] initWithContentsOfURL:playFileURL error:&error];
            [audioPlayer prepareToPlay];
            [audioPlayer play];
            
        }else{
            //arkit使ってる時はiOS11でマナーモードでも再生されるからこいつをそのまま呼べばOK!
            AudioServicesPlaySystemSound(1108);
        }
    }
    
    //Unity側でドキュメント配下に保存した画像をアルバムに保存する
    void _CallSavingPhotoEvent(const char* parentObjName, const char* fileName, const char* callbackName){
        
        NSString* imgDir = [NSHomeDirectory() stringByAppendingPathComponent:@"Documents"];
        NSString* filePath = [NSString stringWithFormat:@"%@/%s",imgDir,fileName];
        
        NSString* _parentObjName =[NSString stringWithFormat:@"%s", parentObjName];
        NSString* _fileName =[NSString stringWithFormat:@"%s", fileName];
        NSString* _callbackName =[NSString stringWithFormat:@"%s", callbackName];
        
        UIImage *image = [UIImage imageWithContentsOfFile:filePath];
        
        ALAssetsLibrary *library = [[ALAssetsLibrary alloc] init];
        NSMutableDictionary *metadata = [[NSMutableDictionary alloc] init];
        [library writeImageToSavedPhotosAlbum:image.CGImage metadata:metadata completionBlock:^(NSURL *assetURL, NSError *error) {
            
            if(error == Nil){
                UnitySendMessage([_parentObjName UTF8String],[_callbackName UTF8String],[_fileName UTF8String]);
            }else{
                NSString *errorCodeStr = [NSString stringWithFormat:@"%ld",error.code];
                
                UnitySendMessage([_parentObjName UTF8String],[_callbackName UTF8String],[errorCodeStr UTF8String]);
            }
        }];
        
    }
    
}
