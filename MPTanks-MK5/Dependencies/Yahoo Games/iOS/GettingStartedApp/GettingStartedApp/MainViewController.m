//
//  MainViewController.m
//  GettingStartedApp
//
//  Created by Arash Rassouli on 3/12/14.
//  Copyright (c) 2014 yahoo. All rights reserved.
//

#import "MainViewController.h"
#import <PlayerIOClient/PlayerIO.h>
#import <PlayerIOClient/PIOClient.h>

@interface MainViewController ()
- (IBAction)connect:(id)sender;
@property (weak, nonatomic) IBOutlet UITextField *txtGameId;
@property (weak, nonatomic) IBOutlet UIButton *btnConnect;

@end

@implementation MainViewController

@synthesize txtGameId, btnConnect;

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
}


- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (IBAction)connect:(id)sender
{
    __block __strong PIOClient* connectedClient;
    [PlayerIO authenticateWithGameId:txtGameId.text
                        connectionId:@"public"
             authenticationArguments:[NSMutableDictionary dictionaryWithObjectsAndKeys:@"YourUserid", @"userId", nil]  playerInsightSegments:nil
                        successBlock:^(PIOClient * client) {
                            connectedClient = client;
                            if (connectedClient){
                                //---- BigDB Example       -------
                                //--------------------------------
                                
                                // load my player object from BigDB
                                __block __strong PIODatabaseObject* myPlayerObject;
                                [connectedClient.bigDB loadMyPlayerObjectWithSuccessBlock:^(PIODatabaseObject *playerObject) {
                                    myPlayerObject = playerObject;
                                    [myPlayerObject setBool:true forProperty:@"awesome"];
                                    //[myPlayerObject save];
                                    [myPlayerObject saveWithSuccessBlock:^{
                                        NSLog(@"Save succeeded!");
                                    } errorBlock:^(PIOError *error) {
                                        NSLog(@"Save failed!");
                                    }];
                                } errorBlock:^(PIOError *error) {
                                    NSLog(@"%@", error);
                                }];
                            }
                        } errorBlock:^(PIOError *error) {
                            NSLog(@"%@", error);
                        }];
}
@end
